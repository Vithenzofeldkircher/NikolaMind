using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.EventSystems.EventTrigger;

public class WireManager : MonoBehaviour
{
    public static WireManager Instance;

    [Header("Configurações")]
    public float fioMaximo = 10f;
    public bool carregandoFio = false;
    public bool missaoConcluida = false;

    [Header("Interação")]
    [SerializeField] private GameObject triggerRecuperarFio; // Arraste aqui um Empty com o seu script Interect

    private void Awake() => Instance = this;

    void Start()
    {
        if (triggerRecuperarFio != null) triggerRecuperarFio.SetActive(false);
    }

    void Update()
    {
        // Verifica se o jogador apertou G para largar o fio
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
        GetComponent<WireRenderer>().InicializarFio(posicaoInicial);
    }

    public void LargarFio()
    {
        carregandoFio = false;

        // Ativa o trigger de interação na posição onde o player soltou o fio
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

        // Esconde o trigger de interação pois o fio já está com o player
        if (triggerRecuperarFio != null) triggerRecuperarFio.SetActive(false);
    }

    public void FinalizarConexao(Vector3 posicaoDestino)
    {
        carregandoFio = false;
        missaoConcluida = true;
        if (triggerRecuperarFio != null) triggerRecuperarFio.SetActive(false);
        GetComponent<WireRenderer>().FixarUltimoPonto(posicaoDestino);
    }

    public bool PodeMoverPara(Vector3 novaPosicao)
    {
        // Se não estiver carregando o fio, pode mover livremente
        if (!carregandoFio) return true;

        WireRenderer renderer = GetComponent<WireRenderer>();
        if (renderer == null || renderer.pontosDoFio.Count < 2) return true;

        // Calcula a distância fixa (todos os segmentos menos o último que move)
        float distanciaFixa = 0;
        for (int i = 0; i < renderer.pontosDoFio.Count - 2; i++)
        {
            distanciaFixa += Vector3.Distance(renderer.pontosDoFio[i], renderer.pontosDoFio[i + 1]);
        }

        // Soma a distância do penúltimo ponto até a posição para onde o player quer ir
        float distanciaSimulada = distanciaFixa + Vector3.Distance(renderer.pontosDoFio[renderer.pontosDoFio.Count - 2], novaPosicao);

        // Permite se: a nova distância for menor que o limite OU se estiver voltando (diminuindo a distância)
        return distanciaSimulada <= fioMaximo || distanciaSimulada < renderer.CalcularDistanciaTotal();
    }

}