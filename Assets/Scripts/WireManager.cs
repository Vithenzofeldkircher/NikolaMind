using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WireManager : MonoBehaviour
{

    public static WireManager Instance;

    private void Awake()
    {
        // Garante que esta instância seja a única acessível
        Instance = this;
    }

    [Header("Configurações")]
    public float fioDisponivel = 10f;
    public bool carregandoFio = false;
    public LayerMask layerColisao; // Marque a Layer das paredes/pilares

    [Header("Componentes")]
    private LineRenderer line;
    private List<Vector3> pontosDoFio = new List<Vector3>();
    [SerializeField] private TextMeshProUGUI textoMetros;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    void Update()
    {
        if (!carregandoFio) return;

        AtualizarFio();
        VerificarColisao();
        VerificarRetorno();
        CalcularConsumo();
        AtualizarUI();
    }

    public void IniciarConexao(float metros, Vector3 posicaoCaixa)
    {
        fioDisponivel = metros;
        carregandoFio = true;
        pontosDoFio.Clear();
        pontosDoFio.Add(posicaoCaixa); // Ponto 0 é a caixa
        pontosDoFio.Add(transform.position); // Ponto 1 é o player
        line.positionCount = 2;
    }

    void AtualizarFio()
    {
        // O último ponto do LineRenderer sempre segue o Player
        pontosDoFio[pontosDoFio.Count - 1] = transform.position;
        line.positionCount = pontosDoFio.Count;
        line.SetPositions(pontosDoFio.ToArray());
    }

    void VerificarColisao()
    {
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];
        Vector3 direcao = transform.position - ultimoPontoFixo;
        float distancia = Vector3.Distance(transform.position, ultimoPontoFixo);

        // Lança um raio do último ponto fixo até o player
        RaycastHit2D hit = Physics2D.Raycast(ultimoPontoFixo, direcao, distancia, layerColisao);

        if (hit.collider != null)
        {
            // Se bateu em algo, adicionamos um novo ponto fixo na quina (um pouco afastado)
            Vector3 pontoQuina = hit.point + (hit.normal * 0.1f);
            pontosDoFio.Insert(pontosDoFio.Count - 1, pontoQuina);
        }
    }

    void VerificarRetorno()
    {
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoPenultimo = pontosDoFio[pontosDoFio.Count - 3];
            Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];

            // Se não houver mais nada entre o player e o penúltimo ponto, removemos a quina atual
            RaycastHit2D hit = Physics2D.Raycast(transform.position, pontoPenultimo - transform.position, Vector3.Distance(transform.position, pontoPenultimo), layerColisao);

            if (hit.collider == null)
            {
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    void CalcularConsumo()
    {
        float comprimentoTotal = 0;
        for (int i = 0; i < pontosDoFio.Count - 1; i++)
        {
            comprimentoTotal += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);
        }

        if (comprimentoTotal > fioDisponivel)
        {
            // Aqui você decide: ou o player para de andar, ou o fio arrebenta
            Debug.Log("Fio esticado no limite!");
        }
    }

    public void FinalizarConexao()
    {
        carregandoFio = false;
        // Opcional: deixar o fio lá ou sumir com ele
    }

    private void AtualizarUI() => textoMetros.text = $"Fio: {fioDisponivel:F1}m";
}