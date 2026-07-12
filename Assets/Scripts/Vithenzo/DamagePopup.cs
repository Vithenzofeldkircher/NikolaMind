using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DamagePopup : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidadeSubida = 2f;
    [SerializeField] private float tempoDeVida = 1f;
    [SerializeField] private float velocidadeFade = 3f;

    private TextMeshProUGUI textoTMP;
    private Color corTexto;

    private void Awake()
    {
        textoTMP = GetComponent<TextMeshProUGUI>();
        corTexto = textoTMP.color;
    }

    public void Configurar(string texto)
    {
        textoTMP.text = texto;
    }

    private void Update()
    {
        // Move o texto para cima ao longo do tempo
        transform.position += Vector3.up * velocidadeSubida * Time.deltaTime;

        tempoDeVida -= Time.deltaTime;

        if (tempoDeVida <= 0)
        {
            // Efeito de desaparecer sumindo gradualmente (Fade Out)
            corTexto.a -= velocidadeFade * Time.deltaTime;
            textoTMP.color = corTexto;

            if (corTexto.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}