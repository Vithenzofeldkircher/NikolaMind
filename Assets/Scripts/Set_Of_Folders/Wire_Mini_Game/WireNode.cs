using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WireNode : MonoBehaviour, IPointerClickHandler
{
    [Header("Componentes Visuais")]
    public Image imagemTerminal;

    [Header("Configuração de Rotação")]
    [Tooltip("Rotação quando o fusível está deitado (horizontal)")]
    [SerializeField] private Vector3 rotacaoDeitado = Vector3.zero;

    [Tooltip("Rotação quando o fusível é erguido ao ser clicado")]
    [SerializeField] private Vector3 rotacaoErguido = new Vector3(0, 0, 90f);

    public WireColorData corAtual { get; private set; }
    public bool ehNoInicial { get; private set; }
    public bool conectado { get; private set; }

    private WireTaskManager gerador;

    private void Awake()
    {
        // Pega automaticamente a imagem do PRÓPRIO objeto se não estiver definida
        if (imagemTerminal == null)
        {
            imagemTerminal = GetComponent<Image>();
        }
    }

    public void ConfigurarNo(WireColorData dadosCor, WireTaskManager manager, bool inicial)
    {
        corAtual = dadosCor;
        gerador = manager;
        ehNoInicial = inicial;
        conectado = false;

        // Garante que a imagem é a deste próprio GameObject
        imagemTerminal = GetComponent<Image>();

        // Força o fusível a iniciar deitado
        ResetarRotacao();

        // 1. Aplica a cor diretamente no componente Image
        if (imagemTerminal != null)
        {
            imagemTerminal.color = dadosCor.cor;
        }

        // 2. Aplica a cor também nas transições do componente Button
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = dadosCor.cor;
            colors.highlightedColor = dadosCor.cor;
            colors.pressedColor = dadosCor.cor * 0.8f;
            colors.selectedColor = dadosCor.cor;
            btn.colors = colors;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (conectado) return;

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