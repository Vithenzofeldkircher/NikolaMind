using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WireNode : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Componentes Visuais")]
    public Image imagemTerminal;
    public LineRenderer linhaFio; // Opcional: Para desenhar o fio visualmente

    public WireColorData corAtual { get; private set; }
    public bool ehNoInicial { get; private set; }
    public bool conectado { get; private set; }

    private WireTaskManager gerador;
    private Vector3 posicaoInicialLinha;

    public void ConfigurarNo(WireColorData dadosCor, WireTaskManager manager, bool inicial)
    {
        corAtual = dadosCor;
        gerador = manager;
        ehNoInicial = inicial;
        conectado = false;

        // Pinta o quadrado com a cor sorteada
        if (imagemTerminal != null)
        {
            imagemTerminal.color = dadosCor.cor;
        }

        // Configura a linha visual se existir
        if (linhaFio != null)
        {
            linhaFio.startColor = dadosCor.cor;
            linhaFio.endColor = dadosCor.cor;
            linhaFio.positionCount = 2;
            posicaoInicialLinha = transform.position;
            linhaFio.SetPosition(0, posicaoInicialLinha);
            linhaFio.SetPosition(1, posicaoInicialLinha);
            linhaFio.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ehNoInicial || conectado) return;

        if (linhaFio != null)
        {
            linhaFio.enabled = true;
            linhaFio.SetPosition(0, transform.position);
            linhaFio.SetPosition(1, GetWorldMousePosition());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!ehNoInicial || conectado) return;

        if (linhaFio != null)
        {
            linhaFio.SetPosition(1, GetWorldMousePosition());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!ehNoInicial || conectado) return;

        // Verifica o que está debaixo do mouse ao soltar o clique
        GameObject objetoAlvo = eventData.pointerCurrentRaycast.gameObject;

        if (objetoAlvo != null)
        {
            WireNode noDestino = objetoAlvo.GetComponent<WireNode>();

            // Valida se o destino é válido e se as cores são iguais
            if (noDestino != null && !noDestino.ehNoInicial && !noDestino.conectado)
            {
                if (noDestino.corAtual.nomeCor == this.corAtual.nomeCor)
                {
                    // Conexão Correta!
                    conectado = true;
                    noDestino.ConectarFio();

                    if (linhaFio != null)
                    {
                        linhaFio.SetPosition(1, noDestino.transform.position);
                    }

                    gerador.RegistrarConexao();
                    return;
                }
            }
        }

        // Se errou ou soltou no vazio, reseta a linha
        if (linhaFio != null)
        {
            linhaFio.enabled = false;
            linhaFio.SetPosition(1, transform.position);
        }
    }

    public void ConectarFio()
    {
        conectado = true;
    }

    private Vector3 GetWorldMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Distância da câmera se for cena 2D/UI
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        return worldPos;
    }
}