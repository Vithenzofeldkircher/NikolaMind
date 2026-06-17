using UnityEngine;
using System.Collections.Generic;

public class WirePhysics : MonoBehaviour
{
    [Header("Configurações do Fio")]
    public LayerMask layerColisao;
    public float distanciaDaQuina = 0.1f; // Define a "espessura" invisível do fio para não entrar na parede.

    [Header("Referências")]
    // Referência ao nosso novo script que só desenha.
    [SerializeField] private WireRenderer visualizadorFio;

    // A lista de pontos agora pertence à física, pois é ela quem dobra e desdobra o fio.
    public List<Vector3> pontosDoFio = new List<Vector3>();

    // Usamos a INTERFACE em vez de Caixa_Enrolar. Assim, o código nunca mais quebra
    // se você adicionar novos tipos de objetos interativos no futuro.
    private IInteragivelFio ultimoObjetoAtivado;

    void Update()
    {
        // Se não houver fio instanciado/iniciado, não fazemos nada.
        if (pontosDoFio.Count == 0) return;

        // Verifica a condição do seu Manager global para saber se o Player está puxando o fio.
        if (WireManager.Instance.carregandoFio)
        {
            // O último ponto da lista é sempre a posição atual deste GameObject (o Player/ponta do fio).
            pontosDoFio[pontosDoFio.Count - 1] = transform.position;

            VerificarColisoes();
            VerificarRetorno();
        }

        // Após a matemática terminar, mandamos o script visual desenhar o resultado.
        visualizadorFio.AtualizarDesenho(pontosDoFio);
    }

    void VerificarColisoes()
    {
        // 1. Obtém o último ponto que já está fixado no cenário (âncora atual do fio).
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];

        // 2. Armazena a posição em tempo real do Player/Ponta do fio.
        Vector3 posAtual = transform.position;

        // 3. Executa o Linecast para detectar se o segmento de fio atual intersectou algum obstáculo.
        RaycastHit2D hit = Physics2D.Linecast(ultimoPontoFixo, posAtual, layerColisao);

        // 4. Se o raio colidiu com algo, significa que o fio precisa dobrar e criar uma nova quina.
        if (hit.collider != null)
        {
            // 5. Inversão de Dependência (DIP/SOLID): Se o objeto implementa nossa interface, nós o notificamos.
            if (hit.collider.TryGetComponent(out IInteragivelFio interagivel))
            {
                interagivel.AoTocarFio();
                ultimoObjetoAtivado = interagivel;
            }


            // 6. Usamos o ClosestPoint do colisor atingido a partir da posição do hit.
            // Isso retorna a coordenada EXATA da superfície ou vértice do objeto (quadrado ou redondo).
            Vector3 pontoBordaExata = hit.collider.ClosestPoint(hit.point);

            // 7. Descobrimos a direção vetorial que vai do centro do objeto até essa borda encontrada.
            Vector3 direcaoParaFora = (pontoBordaExata - hit.collider.bounds.center).normalized;

            // 8. Criamos o ponto final de curvatura pegando a quina exata e empurrando levemente para fora
            // usando a 'distanciaDaQuina' como folga protetiva. Isso evita clipping interno.
            Vector3 pontoDeCurvatura = pontoBordaExata + (direcaoParaFora * distanciaDaQuina);

            // 9. Verificação de redundância: Evita criar múltiplos pontos empilhados no exato mesmo lugar.
            if (Vector3.Distance(pontoDeCurvatura, ultimoPontoFixo) > 0.05f)
            {
                // 10. Insere o novo ponto estratégico como o penúltimo elemento da lista, moldando o fio.
                pontosDoFio.Insert(pontosDoFio.Count - 1, pontoDeCurvatura);
            }
        }
    }

    void VerificarRetorno()
    {
        // 1. O desenrolamento só é logicamente possível se o fio tiver pelo menos 3 pontos (Origem, Dobra, Player).
        if (pontosDoFio.Count > 2)
        {
            // 2. Resgata o ponto de dobra que antecede a dobra atual do fio (antepenúltimo).
            Vector3 pontoAntepenultimo = pontosDoFio[pontosDoFio.Count - 3];
            Vector3 posAtual = transform.position;

            // 3. Traça uma linha de checagem direta entre o antepenúltimo ponto e o Player.
            RaycastHit2D hit = Physics2D.Linecast(pontoAntepenultimo, posAtual, layerColisao);

            // 4. Se hit.collider for nulo, significa que a linha de visão está completamente limpa.
            // O jogador contornou o obstáculo de volta e a dobra atual tornou-se obsoleta.
            if (hit.collider == null)
            {
                // 5. Se o fio estava interagindo com um componente dinâmico, avisa que a conexão cessou.
                if (ultimoObjetoAtivado != null)
                {
                    ultimoObjetoAtivado.AoSoltarFio();
                    ultimoObjetoAtivado = null;
                }

                // 6. Remove a quina atual da lista. O fio estica diretamente para o ponto anterior.
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    // --- Métodos Auxiliares Antigos Mantidos ---

    public void InicializarFio(Vector3 posInicial)
    {
        pontosDoFio.Clear();
        pontosDoFio.Add(posInicial); // Ponto 0: A base de onde o fio saiu
        pontosDoFio.Add(transform.position); // Ponto 1: A ponta do fio com o Player
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