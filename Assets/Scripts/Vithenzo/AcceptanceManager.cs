using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AcceptanceManager : MonoBehaviour
{
    [Header("Configurações de UI")]
    public Slider acceptanceSlider; // Use apenas este agora
    public Image fillImage;

    [Header("Configurações de Limites")]
    public float maxAcceptance = 100f;
    public float minAcceptance = 0f;

    void Start()
    {
        // Configura o slider
        if (acceptanceSlider != null)
        {
            acceptanceSlider.maxValue = maxAcceptance;
            acceptanceSlider.minValue = minAcceptance;
        }

        // 2. BUSCA O VALOR SALVO: Essencial para quando volta do minigame
        if (Points_Maneger.Instance != null)
        {
            // Pega o valor que sobreviveu à troca de cena e aplica na UI
            UpdateUI(Points_Maneger.Instance.pontosAtuais);
            Debug.Log("Barra atualizada com pontos salvos: " + Points_Maneger.Instance.pontosAtuais);
        }

        // SINCRONIZAÇÃO: Busca o valor que está salvo no ScoreManager ao iniciar a cena
        if (Points_Maneger.Instance != null)
        {
            UpdateUI(Points_Maneger.Instance.pontosAtuais);
        }
    }

    // Este método agora recebe o valor total vindo do ScoreManager
    public void UpdateAcceptance(float totalPoints)
    {
        UpdateUI(totalPoints);
        CheckEndGame(totalPoints);
    }

    void UpdateUI(float value)
    {
        if (acceptanceSlider != null)
        {
            acceptanceSlider.value = value;

            // Muda a cor de Vermelho para Verde baseado no valor
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.red, Color.green, value / maxAcceptance);
            }
        }
    }

    private void CheckEndGame(float value)
    {
        if (value >= maxAcceptance)
        {
            SceneManager.LoadScene("FinalBom");
        }
        else if (value <= minAcceptance)
        {
            SceneManager.LoadScene("FinalRuim");
        }
    }
}