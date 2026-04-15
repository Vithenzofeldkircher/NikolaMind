using UnityEngine;
using TMPro;

public class WireUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMetros;
    private WireRenderer wireRenderer;

    void Start() => wireRenderer = GetComponent<WireRenderer>();

    void Update()
    {
        if (WireManager.Instance.missaoConcluida)
        {
            textoMetros.text = "CONECTADO!";
            return;
        }

        if (WireManager.Instance.carregandoFio)
        {
            float dist = wireRenderer.CalcularDistanciaTotal();
            float sobra = Mathf.Max(0, WireManager.Instance.fioMaximo - dist);
            textoMetros.text = $"Fio: {sobra:F1}m";
        }
    }
}