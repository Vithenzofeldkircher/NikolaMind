using UnityEngine;
using System.Collections.Generic;

public class WirePhysics : MonoBehaviour
{
    [Header("Configurações do Fio")]
    public LayerMask layerColisao;
    public float distanciaDaQuina = 0.15f; // Aumentado levemente para garantir folga em BoxCollider2D

    [Header("Referências")]
    [SerializeField] private WireRenderer visualizadorFio;

    public List<Vector3> pontosDoFio = new List<Vector3>();
    private IInteragivelFio ultimoObjetoAtivado;

    void Update()
    {
        if (pontosDoFio.Count == 0) return;

        if (WireManager.Instance.carregandoFio)
        {
            pontosDoFio[pontosDoFio.Count - 1] = transform.position;

            VerificarColisoes();
            VerificarRetorno();
        }

        visualizadorFio.AtualizarDesenho(pontosDoFio);
    }

    void VerificarColisoes()
    {
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];
        Vector3 posAtual = transform.position;

        // Executa o Linecast para detectar colisão com o cenário
        RaycastHit2D hit = Physics2D.Linecast(ultimoPontoFixo, posAtual, layerColisao);

        if (hit.collider != null)
        {
            // Inversão de Dependência (SOLID)
            if (hit.collider.TryGetComponent(out IInteragivelFio interagivel))
            {
                interagivel.AoTocarFio();
                ultimoObjetoAtivado = interagivel;
            }

            Vector3 pontoDeCurvatura;

            // Se for um BoxCollider2D, calculamos a quina matemática perfeita para não atravessar
            if (hit.collider is BoxCollider2D boxCollider)
            {
                pontoDeCurvatura = CalcularQuinaBoxCollider(boxCollider, hit.point);
            }
            else
            {
                // Fallback para outros tipos de colisores (como Circle ou Polygon)
                Vector3 pontoBordaExata = hit.collider.ClosestPoint(hit.point);
                Vector3 direcaoParaFora = (pontoBordaExata - hit.collider.bounds.center).normalized;
                pontoDeCurvatura = pontoBordaExata + (direcaoParaFora * distanciaDaQuina);
            }

            // Evita redundância de pontos colados
            if (Vector3.Distance(pontoDeCurvatura, ultimoPontoFixo) > 0.05f)
            {
                // Insere a nova quina perfeitamente antes da posição do jogador
                pontosDoFio.Insert(pontosDoFio.Count - 1, pontoDeCurvatura);
            }
        }
    }

    void VerificarRetorno()
    {
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoAntepenultimo = pontosDoFio[pontosDoFio.Count - 3];
            Vector3 posAtual = transform.position;

            // Faz uma varredura para ver se o caminho até o antepenúltimo ponto limpou
            RaycastHit2D hit = Physics2D.Linecast(pontoAntepenultimo, posAtual, layerColisao);

            if (hit.collider == null)
            {
                if (ultimoObjetoAtivado != null)
                {
                    ultimoObjetoAtivado.AoSoltarFio();
                    ultimoObjetoAtivado = null;
                }

                // Remove a quina, desenroscando o fio
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    /// <summary>
    /// SRP: Método focado unicamente em extrair a quina geométrica correta de um BoxCollider2D
    /// </summary>
    private Vector3 CalcularQuinaBoxCollider(BoxCollider2D box, Vector2 pontoImpacto)
    {
        Bounds bounds = box.bounds;

        // Lista as 4 quinas mundiais do BoxCollider2D
        Vector3[] quinas = new Vector3[]
        {
            new Vector3(bounds.min.x, bounds.max.y, 0), // Superior Esquerda
            new Vector3(bounds.max.x, bounds.max.y, 0), // Superior Direita
            new Vector3(bounds.min.x, bounds.min.y, 0), // Inferior Esquerda
            new Vector3(bounds.max.x, bounds.min.y, 0)  // Inferior Direita
        };

        // Encontra qual das 4 quinas está mais próxima do ponto real onde o fio encostou
        Vector3 quinaMaisProxima = quinas[0];
        float menorDistancia = Vector2.Distance(pontoImpacto, quinaMaisProxima);

        for (int i = 1; i < quinas.Length; i++)
        {
            float dist = Vector2.Distance(pontoImpacto, quinas[i]);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                quinaMaisProxima = quinas[i];
            }
        }

        // Calcula o vetor de deslocamento para fora a partir do centro do collider
        Vector3 direcaoOffset = (quinaMaisProxima - bounds.center).normalized;

        // Retorna a quina com a folga necessária para o LineRenderer não clipar na parede
        return quinaMaisProxima + (direcaoOffset * distanciaDaQuina);
    }

    // --- Métodos Auxiliares Mantidos ---
    public void InicializarFio(Vector3 posInicial)
    {
        pontosDoFio.Clear();
        pontosDoFio.Add(posInicial);
        pontosDoFio.Add(transform.position);
    }

    public void FixarUltimoPonto(Vector3 pos)
    {
        pontosDoFio[pontosDoFio.Count - 1] = pos;
    }

    public float CalcularDistanciaTotal()
    {
        float d = 0;
        for (int i = 0; i < pontosDoFio.Count - 1; i++)
        {
            d += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);
        }
        return d;
    }
}