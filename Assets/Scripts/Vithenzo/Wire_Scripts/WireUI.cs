using UnityEngine;
using TMPro;

public class WireUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMetros;

    [Header("Referências do Sistema")]
    // Transformado em SerializeField para você arrastar o objeto do Player/Fio no Inspector,
    // mitigando o bug do GetComponent retornar null caso a UI esteja no Canvas.
    [SerializeField] private WirePhysics wirePhysics;
    void Start()
    {
        wirePhysics = GetComponent<WirePhysics>();

        // Começa com o texto desativado por segurança
        if (textoMetros != null)
            textoMetros.gameObject.SetActive(false);
    }

    void Update()
    {
        // 1. Verificações de segurança
        if (textoMetros == null || wirePhysics == null || WireManager.Instance == null) return;

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
            // CORREÇÃO DO BUG: Chamando o método a partir da INSTÂNCIA (wirePhysics) e não da classe.
            float dist = wirePhysics.CalcularDistanciaTotal();

            // Calcula quanta fiação resta
            float sobra = Mathf.Max(0, WireManager.Instance.fioMaximo - dist);

            // Exibe apenas a quantidade (Ex: "8.5m")
            textoMetros.text = $"{sobra:F1}m";
        }
    }
}