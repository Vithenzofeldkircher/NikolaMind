using UnityEngine;
using TMPro;

public class WireUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMetros;

    [Header("Referências do Sistema")]
    [SerializeField, Tooltip("Arraste a instância de WirePhysics (pode estar em outro GameObject).")]
    private WirePhysics wirePhysics;

    // Cache para evitar escrever o mesmo texto repetidas vezes
    private string _ultimoTexto = string.Empty;

    private void OnValidate()
    {
        // Facilita configuração no Inspector durante edição
        if (wirePhysics == null)
            wirePhysics = GetComponent<WirePhysics>();

        if (textoMetros == null)
            textoMetros = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void Start()
    {
        // Tentativas seguras de resolução em tempo de execução
        if (wirePhysics == null)
            wirePhysics = GetComponent<WirePhysics>() ?? GetComponent<WirePhysics>();

        if (textoMetros != null)
            textoMetros.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Segurança: aborta cedo se faltar referência
        if (textoMetros == null || wirePhysics == null || WireManager.Instance == null)
            return;

        // Mostrar só enquanto o jogador estiver carregando fio e a missão não estiver concluída
        bool deveMostrarTexto = WireManager.Instance.carregandoFio && !WireManager.Instance.missaoConcluida;

        // Ativa/desativa o GameObject do texto apenas quando necessário
        if (textoMetros.gameObject.activeSelf != deveMostrarTexto)
            textoMetros.gameObject.SetActive(deveMostrarTexto);

        if (!deveMostrarTexto)
        {
            // limpa cache para forçar atualização quando reativar
            _ultimoTexto = string.Empty;
            return;
        }

        // Obtém distância atual do fio a partir da instância de física
        float distanciaTotal = wirePhysics.CalcularDistanciaTotal();

        // Calcula o quanto resta, garantindo não-negativo
        float sobra = Mathf.Max(0f, WireManager.Instance.fioMaximo - distanciaTotal);

        // Formata texto (ex: "8.5m") e só atualiza quando diferente do atual (reduz GC e trabalho do TMP)
        string novoTexto = $"{sobra:F1}m";
        if (!novoTexto.Equals(_ultimoTexto))
        {
            textoMetros.text = novoTexto;
            _ultimoTexto = novoTexto;
        }
    }
}