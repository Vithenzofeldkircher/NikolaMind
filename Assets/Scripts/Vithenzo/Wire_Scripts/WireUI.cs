using UnityEngine;
using TMPro;

public class WireUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMetros;
    private WireRenderer wireRenderer;

    void Start()
    {
        wireRenderer = GetComponent<WireRenderer>();

        // Começa com o texto desativado por segurança
        if (textoMetros != null)
            textoMetros.gameObject.SetActive(false);
    }

    void Update()
    {
        // 1. Verificações de segurança
        if (textoMetros == null || wireRenderer == null || WireManager.Instance == null) return;

        // 2. Lógica de Ativação: O texto só aparece se estiver carregando o fio
        // E desaparece se a missão for concluída ou se o fio for largado
        bool deveMostrarTexto = WireManager.Instance.carregandoFio && !WireManager.Instance.missaoConcluida;

        // Ativa ou desativa o objeto de texto no Canvas
        if (textoMetros.gameObject.activeSelf != deveMostrarTexto)
        {
            textoMetros.gameObject.SetActive(deveMostrarTexto);
        }

        // 3. Atualização do conteúdo (Só roda se o texto estiver visível)
        if (deveMostrarTexto)
        {
            float dist = wireRenderer.CalcularDistanciaTotal();
            float sobra = Mathf.Max(0, WireManager.Instance.fioMaximo - dist);

            // Exibe apenas a quantidade (Ex: "8.5m")
            textoMetros.text = $"{sobra:F1}m";
        }
    }
}