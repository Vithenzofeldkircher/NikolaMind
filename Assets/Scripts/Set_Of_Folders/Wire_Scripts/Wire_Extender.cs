using UnityEngine;

public class Wire_Extender : MonoBehaviour, IInteragivelFioDiscreto
{
    [Header("Configurações do Extensor")]
    [SerializeField] private float bonusMetros = 5f;
    [SerializeField] private Color corFioConectado = Color.green;

    private Color corFioOriginal;
    private bool jaAtivado = false;
    private LineRenderer lineRendererRef;

    // Guardamos a referência do WirePhysics que realizou a conexão para poder restaurar a cor depois
    private WirePhysics physicsHandlerAlvo;

    public void InteragirComFio()
    {
        // Se ainda não foi ativado, conecta o cabo e concede o bônus
        if (!jaAtivado)
        {
            ConectarExtensor();
        }
        else
        {
            // Se já estava ativado e o jogador clicou novamente, desconecta o cabo
            DesconectarExtensor();
        }
    }

    private void ConectarExtensor()
    {
        if (WireManager.Instance == null) return;

        // Tenta obter o manipulador de física através do Manager global de forma dinâmica
        physicsHandlerAlvo = WireManager.Instance.GetComponent<WirePhysics>();
        if (physicsHandlerAlvo == null) return;

        // Tenta obter o LineRenderer para realizar a troca de cores cosmética
        lineRendererRef = physicsHandlerAlvo.GetComponent<LineRenderer>();

        if (lineRendererRef != null)
        {
            // Salva a cor original antes de aplicar a nova cor de feedback
            corFioOriginal = lineRendererRef.startColor;
            lineRendererRef.startColor = corFioConectado;
            lineRendererRef.endColor = corFioConectado;
        }

        // Aplica as regras de negócio no Manager
        WireManager.Instance.fioMaximo += bonusMetros;
        jaAtivado = true;

        Debug.Log($"[Extensor] Fio conectado! Capacidade aumentada em +{bonusMetros}m.");
    }

    private void DesconectarExtensor()
    {
        if (WireManager.Instance == null || physicsHandlerAlvo == null) return;

        // Remove o bônus concedido anteriormente de forma limpa
        WireManager.Instance.fioMaximo -= bonusMetros;

        // Restaura a paleta de cores original do LineRenderer
        if (lineRendererRef != null)
        {
            lineRendererRef.startColor = corFioOriginal;
            lineRendererRef.endColor = corFioOriginal;
        }

        jaAtivado = false;
        physicsHandlerAlvo = null;

        Debug.Log("[Extensor] Fio desconectado. Bônus de metros removido.");
    }

    // Método auxiliar público para que outros scripts verifiquem se o extensor está em uso
    public bool EstáAtivo() => jaAtivado;
}