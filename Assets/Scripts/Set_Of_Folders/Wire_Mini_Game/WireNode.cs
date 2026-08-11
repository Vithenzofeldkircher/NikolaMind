using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WireNode : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuração de Rotação")]
    [Tooltip("Rotação quando o fusível está deitado (0 graus)")]
    [SerializeField] private Vector3 rotacaoDeitado = Vector3.zero;

    [Tooltip("Rotação quando o fusível é erguido (90 graus)")]
    [SerializeField] private Vector3 rotacaoErguido = new Vector3(0, 0, 90f);

    public WireColorData corAtual { get; private set; }
    public bool ehNoInicial { get; private set; }
    public bool conectado { get; private set; }

    private Image imagemTerminal;
    private Button botao;
    private WireTaskManager gerador;

    private void Awake()
    {
        imagemTerminal = GetComponent<Image>();
        botao = GetComponent<Button>();
    }

    public void ConfigurarNo(WireColorData dadosCor, WireTaskManager manager, bool inicial)
    {
        corAtual = dadosCor;
        gerador = manager;
        ehNoInicial = inicial;
        conectado = false;

        if (imagemTerminal == null) imagemTerminal = GetComponent<Image>();
        if (botao == null) botao = GetComponent<Button>();

        // Força a transparência (Alpha) para 1 (100% visível)
        Color corFinal = dadosCor.cor;
        corFinal.a = 1f;

        // 1. Aplica a cor diretamente na Image do próprio botão
        if (imagemTerminal != null)
        {
            imagemTerminal.color = corFinal;
        }

        // 2. Configura a transição do botão para usar a mesma cor
        if (botao != null)
        {
            botao.targetGraphic = imagemTerminal;
            ColorBlock colors = botao.colors;
            colors.normalColor = corFinal;
            colors.highlightedColor = corFinal * 1.1f;
            colors.pressedColor = corFinal * 0.8f;
            colors.selectedColor = corFinal;
            colors.colorMultiplier = 1f;
            botao.colors = colors;
        }

        // Inicia deitado (0 graus)
        ResetarRotacao();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (conectado) return;

        Debug.Log($"[WireNode] Clicou no botão: {gameObject.name} | É Inicial? {ehNoInicial} | Cor: {corAtual.nomeCor}");

        if (ehNoInicial)
        {
            ErguerFusivel();
            gerador.SelecionarNoInicial(this);
        }
        else
        {
            gerador.TentarConectar(this);
        }
    }

    public void ErguerFusivel()
    {
        transform.localRotation = Quaternion.Euler(rotacaoErguido);
    }

    public void ResetarRotacao()
    {
        transform.localRotation = Quaternion.Euler(rotacaoDeitado);
    }

    public void ConfirmarConexao()
    {
        conectado = true;
    }
}