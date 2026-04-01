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

    void VerificarColisoes() { /* Raycast para detectar quinas */ }

    public void FinalizarConexao() { carregandoFio = false; }

    private void AtualizarUI()
    {
        if (textoMetros != null)
            textoMetros.text = carregandoFio ? $"Fio: {fioAtual:F1}m" : "";
    }
}