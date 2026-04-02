using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AcceptanceManager : MonoBehaviour
{
    [Header("Configurações de Pontos")]
    public Slider acceptanceSlider;
    public float currentAcceptance = 50f; // Começa no meio
    public float maxAcceptance = 100f;
    public float minAcceptance = 0f;

    [Header("Referências UI")]
    public Slider AcceptanceSlider;
    public Image fillImage;

    void Start()
    {
        // Inicializa a barra com o valor atual
        AcceptanceSlider.maxValue = maxAcceptance;
        AcceptanceSlider.minValue = minAcceptance;
        UpdateUI();
    }

    // Método para ser chamado quando uma quest termina
    public void UpdateAcceptance(float amount)
    {
        currentAcceptance += amount;

        // Impede que o valor passe do máximo ou do mínimo
        currentAcceptance = Mathf.Clamp(currentAcceptance, minAcceptance, maxAcceptance);

        UpdateUI();
        CheckEndGame();
    }

    void UpdateUI()
    {
        acceptanceSlider.value = currentAcceptance;

        // BÔNUS: Muda a cor de Vermelho (ruim) para Verde (bom)
        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(Color.red, Color.green, currentAcceptance / maxAcceptance);
        }
    }

    private void CheckEndGame()
    {
        if (currentAcceptance >= maxAcceptance)
        {
            Debug.Log("Voce ganhou");
            SceneManager.LoadScene("FinalBom");
        }
        else if (currentAcceptance <= minAcceptance)
        {
            Debug.Log("Voce perdeu");
            SceneManager.LoadScene("FinalRuim");
        }
    }
}