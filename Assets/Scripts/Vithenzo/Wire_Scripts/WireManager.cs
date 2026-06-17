using UnityEngine;

public class WireManager : MonoBehaviour
{
    public static WireManager Instance;

    [Header("Configurações")]
    public float fioMaximo;
    public bool carregandoFio = false;
    public bool missaoConcluida = false;

    [Header("Referências")]
    [SerializeField] private GameObject triggerRecuperarFio;
    // Agora o Manager aponta para a Física, não para o Renderer
    [SerializeField] private WirePhysics physicsHandler;

    private void Awake() => Instance = this;

    void Start()
    {
        if (triggerRecuperarFio != null) triggerRecuperarFio.SetActive(false);
    }

    void Update()
    {
        if (carregandoFio && Input.GetKeyDown(KeyCode.G))
        {
            LargarFio();
        }
    }

    public void IniciarConexao(float metros, Vector3 posicaoInicial)
    {
        if (missaoConcluida) return;
        fioMaximo = metros;
        carregandoFio = true;

        // Chamada delegada para a Física
        physicsHandler.InicializarFio(posicaoInicial);
    }

    public void LargarFio()
    {
        carregandoFio = false;

        if (triggerRecuperarFio != null)
        {
            triggerRecuperarFio.transform.position = transform.position;
            triggerRecuperarFio.SetActive(true);
        }
    }

    public void RetomarFio()
    {
        if (missaoConcluida) return;
        carregandoFio = true;

        if (triggerRecuperarFio != null) triggerRecuperarFio.SetActive(false);
    }

    public void FinalizarConexao(Vector3 posicaoDestino)
    {
        carregandoFio = false;
        missaoConcluida = true;
        if (triggerRecuperarFio != null) triggerRecuperarFio.SetActive(false);

        // Chamada delegada para a Física
        physicsHandler.FixarUltimoPonto(posicaoDestino);
    }

    public bool PodeMoverPara(Vector3 novaPosicao)
    {
        if (!carregandoFio) return true;

        // Acessamos os dados através da Física (que é o dono da lista de pontos)
        var pontos = physicsHandler.pontosDoFio;
        if (pontos == null || pontos.Count < 2) return true;

        float distanciaFixa = 0;
        for (int i = 0; i < pontos.Count - 2; i++)
        {
            distanciaFixa += Vector3.Distance(pontos[i], pontos[i + 1]);
        }

        float distanciaSimulada = distanciaFixa + Vector3.Distance(pontos[pontos.Count - 2], novaPosicao);

        // Chamada delegada para o método de cálculo da Física
        return distanciaSimulada <= fioMaximo || distanciaSimulada < physicsHandler.CalcularDistanciaTotal();
    }
}