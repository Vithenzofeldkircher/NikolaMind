using UnityEngine;
using System.Collections.Generic;

public class WirePhysics : MonoBehaviour
{
    [Header("Configurações do Fio")]
    public LayerMask layerColisao;
    public float distanciaDaQuina = 0.15f;

    [Header("Referências")]
    [SerializeField] private WireRenderer visualizadorFio;

    public List<Vector3> pontosDoFio = new List<Vector3>();

    // SOLID (SRP/DIP): Histórico de interações vinculado a cada quina gerada no fio.
    // Substitui o "ultimoObjetoAtivado" para suportar múltiplas voltas na mesma caixa.
    private List<IInteragivelFio> historicoInteracoes = new List<IInteragivelFio>();

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

        Vector3 direcaoRaycast = (posAtual - ultimoPontoFixo).normalized;
        Vector3 origemRaio = ultimoPontoFixo + (direcaoRaycast * 0.05f);

        RaycastHit2D hit = Physics2D.Linecast(origemRaio, posAtual, layerColisao);

        if (hit.collider != null)
        {
            IInteragivelFio interagivelEncontrado = null;

            if (hit.collider.TryGetComponent(out IInteragivelFio interagivel))
            {
                interagivel.AoTocarFio();
                interagivelEncontrado = interagivel;
            }

            Vector3 pontoDeCurvatura;

            if (hit.collider is BoxCollider2D boxCollider)
            {
                pontoDeCurvatura = CalcularQuinaBoxCollider(boxCollider, hit.point, ultimoPontoFixo);
            }
            else
            {
                Vector3 pontoBordaExata = hit.collider.ClosestPoint(hit.point);
                Vector3 direcaoParaFora = (pontoBordaExata - hit.collider.bounds.center).normalized;
                pontoDeCurvatura = pontoBordaExata + (direcaoParaFora * distanciaDaQuina);
            }

            // Garante que não encavale pontos e registra o histórico da quina
            if (Vector3.Distance(pontoDeCurvatura, ultimoPontoFixo) > 0.05f)
            {
                pontosDoFio.Insert(pontosDoFio.Count - 1, pontoDeCurvatura);

                // CORREÇÃO: Salvamos quem foi o objeto tocado NESSA quina específica
                historicoInteracoes.Add(interagivelEncontrado);
            }
        }
    }

    void VerificarRetorno()
    {
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoAntepenultimo = pontosDoFio[pontosDoFio.Count - 3];
            Vector3 posAtual = transform.position;

            RaycastHit2D hit = Physics2D.Linecast(pontoAntepenultimo, posAtual, layerColisao);

            if (hit.collider == null)
            {
                // CORREÇÃO: Recuperamos o objeto que estava atrelado à última quina criada
                int ultimoIndice = historicoInteracoes.Count - 1;
                if (ultimoIndice >= 0)
                {
                    IInteragivelFio objetoAncorado = historicoInteracoes[ultimoIndice];
                    objetoAncorado?.AoSoltarFio(); // Avisa a caixa (se houver uma)

                    historicoInteracoes.RemoveAt(ultimoIndice);
                }

                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    private Vector3 CalcularQuinaBoxCollider(BoxCollider2D box, Vector2 pontoImpacto, Vector3 ultimoPontoFixo)
    {
        Bounds bounds = box.bounds;

        Vector3[] quinas = new Vector3[]
        {
            new Vector3(bounds.min.x, bounds.max.y, 0),
            new Vector3(bounds.max.x, bounds.max.y, 0),
            new Vector3(bounds.min.x, bounds.min.y, 0),
            new Vector3(bounds.max.x, bounds.min.y, 0)
        };

        Vector3 melhorQuina = Vector3.zero;
        float menorDistancia = float.MaxValue;

        for (int i = 0; i < quinas.Length; i++)
        {
            Vector3 direcaoOffset = (quinas[i] - bounds.center).normalized;
            Vector3 quinaCalculada = quinas[i] + (direcaoOffset * distanciaDaQuina);

            if (Vector3.Distance(quinaCalculada, ultimoPontoFixo) < 0.1f)
                continue;

            float dist = Vector2.Distance(pontoImpacto, quinas[i]);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                melhorQuina = quinaCalculada;
            }
        }

        if (menorDistancia == float.MaxValue)
        {
            Vector3 direcaoOffset = (quinas[0] - bounds.center).normalized;
            return quinas[0] + (direcaoOffset * distanciaDaQuina);
        }

        return melhorQuina;
    }

    public void InicializarFio(Vector3 posInicial)
    {
        pontosDoFio.Clear();
        historicoInteracoes.Clear(); // Limpa o histórico ao começar um novo fio

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