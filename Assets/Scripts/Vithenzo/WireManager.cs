using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WireManager : MonoBehaviour
{
    public static WireManager Instance;

    [Header("Configurações do Fio")]
    public float fioMaximo = 10f;
    public float fioAtual = 0f;
    public bool carregandoFio = false;
    public LayerMask layerColisao;

    [Header("Componentes")]
    private LineRenderer line;
    private List<Vector3> pontosDoFio = new List<Vector3>();
    [SerializeField] private TextMeshProUGUI textoMetros;

    public bool missaoConcluida = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        line = GetComponent<LineRenderer>();
        // Configura o LineRenderer para usar cores do Inspector (Gradient)
        line.positionCount = 0;

        if (textoMetros != null) textoMetros.text = "";
    }

    void Update()
    {
        // Tecla G para largar o fio
        if (carregandoFio && Input.GetKeyDown(KeyCode.G))
        {
            LargarFio();
        }

        // Se o fio estiver na cena (mesmo largado), precisamos desenhá-lo
        if (pontosDoFio.Count > 0)
        {
            DesenharFio();
        }

        if (!carregandoFio) return;

        // Só executa lógica de movimento/distância se estiver carregando
        VerificarColisoes();
        VerificarRetorno();
        CalcularDistancia();
        AtualizarUI();
    }

    void DesenharFio()
    {
        if (carregandoFio)
        {
            // O último ponto segue o player
            pontosDoFio[pontosDoFio.Count - 1] = transform.position;
        }

        line.positionCount = pontosDoFio.Count;
        line.SetPositions(pontosDoFio.ToArray());
    }

    void CalcularDistancia()
    {
        float distanciaTotal = 0;
        for (int i = 0; i < pontosDoFio.Count - 1; i++)
        {
            distanciaTotal += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);
        }
        fioAtual = Mathf.Max(0, fioMaximo - distanciaTotal);
    }

    public bool PodeMoverPara(Vector3 novaPosicao)
    {
        if (!carregandoFio) return true;

        float distanciaSimulada = 0;
        for (int i = 0; i < pontosDoFio.Count - 2; i++)
            distanciaSimulada += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);

        distanciaSimulada += Vector3.Distance(pontosDoFio[pontosDoFio.Count - 2], novaPosicao);

        // Permite se: a nova distância for menor que o limite OU se estiver diminuindo (voltando)
        return distanciaSimulada <= fioMaximo || distanciaSimulada < GetDistanciaTotalAtual();
    }

    float GetDistanciaTotalAtual()
    {
        float d = 0;
        for (int i = 0; i < pontosDoFio.Count - 1; i++)
            d += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);
        return d;
    }

    public void IniciarConexao(float metros, Vector3 posicaoInicial)
    {
        fioMaximo = metros;
        carregandoFio = true;

        if (missaoConcluida) return; // Impede de iniciar se já acabou

        fioMaximo = metros;
        carregandoFio = true; 

        if (pontosDoFio.Count < 2)
        {
            pontosDoFio.Clear();
            pontosDoFio.Add(posicaoInicial);
            pontosDoFio.Add(transform.position);
        }
        // Se já houver pontos, o Update cuidará de prender o último ponto ao player novamente
    }

    public void LargarFio()
    {
        carregandoFio = false;
        // O último ponto fica parado onde o player o soltou
        pontosDoFio[pontosDoFio.Count - 1] = transform.position;
        Debug.Log("Fio largado no chão.");
    }

    public void FinalizarConexao(Vector3 posicaoDestino)
    {
        carregandoFio = false;
        missaoConcluida = true;

        pontosDoFio[pontosDoFio.Count - 1] = posicaoDestino;
        DesenharFio();

        if (textoMetros != null)
            textoMetros.text = "CONECTADO!";

        if (Mission_Pass.Instance != null)
        {
            Mission_Pass.Instance.AtivarVitoria();
        }
    }

    // Lógica de Raycast para quinas (Mantenha como está)
    void VerificarColisoes()
    {
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];
        Vector3 direcao = transform.position - ultimoPontoFixo;
        float distancia = Vector3.Distance(transform.position, ultimoPontoFixo);
        RaycastHit2D hit = Physics2D.Raycast(ultimoPontoFixo, direcao, distancia, layerColisao);

        if (hit.collider != null)
        {
            Vector3 pontoQuina = (Vector3)hit.point + ((Vector3)hit.normal * 0.1f);
            pontosDoFio.Insert(pontosDoFio.Count - 1, pontoQuina);
        }
    }

    void VerificarRetorno()
    {
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoPenultimo = pontosDoFio[pontosDoFio.Count - 3];
            float distancia = Vector3.Distance(transform.position, pontoPenultimo);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, pontoPenultimo - transform.position, distancia, layerColisao);

            if (hit.collider == null)
            {
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    private void AtualizarUI()
    {
        if (textoMetros != null)
            textoMetros.text = carregandoFio ? $"Fio: {fioAtual:F1}m" : "";
    }
}