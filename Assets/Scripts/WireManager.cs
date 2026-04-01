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
    public bool podeMover = true; // O script de movimento vai ler isso
    public LayerMask layerColisao;

    [Header("Componentes")]
    private LineRenderer line;
    private List<Vector3> pontosDoFio = new List<Vector3>();
    [SerializeField] private TextMeshProUGUI textoMetros;

    private void Awake() => Instance = this;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    void Update()
    {
        if (!carregandoFio) { podeMover = true; return; }

        AtualizarPontosFio();
        VerificarColisoes();
        CalcularDistanciaETravar();
        AtualizarUI();
        VerificarRetorno();
    }

    public void IniciarConexao(float metros, Vector3 posicaoInicial)
    {
        fioMaximo = metros;
        carregandoFio = true;
        pontosDoFio.Clear();
        pontosDoFio.Add(posicaoInicial); // Ponto da caixa
        pontosDoFio.Add(transform.position); // Ponto do player
    }

    void CalcularDistanciaETravar()
    {
        float distanciaTotal = 0;
        // Soma a distância de todos os segmentos (curvas do fio)
        for (int i = 0; i < pontosDoFio.Count - 1; i++)
        {
            distanciaTotal += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);
        }

        fioAtual = fioMaximo - distanciaTotal;

        // Se a distância total for maior ou igual ao limite, trava o player
        if (distanciaTotal >= fioMaximo)
        {
            podeMover = false;
            fioAtual = 0;
        }
        else
        {
            podeMover = true;
        }
    }

    void AtualizarPontosFio()
    {
        pontosDoFio[pontosDoFio.Count - 1] = transform.position;
        line.positionCount = pontosDoFio.Count;
        line.SetPositions(pontosDoFio.ToArray());
    }

    void VerificarColisoes()
    {
        // Pegamos o último ponto "fixo" do fio (onde ele dobrou por último)
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];
        Vector3 direcao = transform.position - ultimoPontoFixo;
        float distancia = Vector3.Distance(transform.position, ultimoPontoFixo);

        // Lançamos um raio laser invisível entre o player e o último ponto fixo
        RaycastHit2D hit = Physics2D.Raycast(ultimoPontoFixo, direcao, distancia, layerColisao);

        if (hit.collider != null)
        {
            // Se o raio bateu em algo, o fio "dobrou". 
            // Adicionamos um novo ponto na lista, exatamente na quina do objeto.
            // O offset (0.1f) evita que o fio fique "dentro" da parede.
            Vector3 pontoQuina = (Vector3)hit.point + ((Vector3)hit.normal * 0.1f);

            // Insere o ponto antes da posição atual do player
            pontosDoFio.Insert(pontosDoFio.Count - 1, pontoQuina);
        }
    }

    void VerificarRetorno()
    {
        // Se o fio tiver dobras (mais de 2 pontos)
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoPenultimo = pontosDoFio[pontosDoFio.Count - 3];

            // Lançamos um raio entre o player e o ponto ANTERIOR à última dobra
            float distancia = Vector3.Distance(transform.position, pontoPenultimo);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, pontoPenultimo - transform.position, distancia, layerColisao);

            // Se o caminho estiver livre, significa que o fio "desenrolou"
            if (hit.collider == null)
            {
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    public void FinalizarConexao() { carregandoFio = false; }

    private void AtualizarUI()
    {
        if (textoMetros != null)
            textoMetros.text = carregandoFio ? $"Fio: {fioAtual:F1}m" : "";
    }
}