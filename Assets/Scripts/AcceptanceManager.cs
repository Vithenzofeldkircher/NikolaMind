using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AcceptanceManager : MonoBehaviour
{
    public Slider acceptanceSlider;
    public float currentAcceptance = 50f; // Começa no meio
    public float maxAcceptance = 100f;
    public float minAcceptance = 0f;

    void Start()
    {
        // Inicializa a barra com o valor atual
        acceptanceSlider.maxValue = maxAcceptance;
        acceptanceSlider.value = currentAcceptance;
    }

    // Método para ser chamado quando uma quest termina
    public void UpdateAcceptance(float amount)
    {
        currentAcceptance += amount;

        // Garante que o valor não saia dos limites
        currentAcceptance = Mathf.Clamp(currentAcceptance, minAcceptance, maxAcceptance);

        // Atualiza a barra visualmente
        acceptanceSlider.value = currentAcceptance;

        CheckEndGame();
    }

    private void CheckEndGame()
    {
        if (currentAcceptance >= maxAcceptance)
        {
            SceneManager.LoadScene("FinalBom");
        }
        else if (currentAcceptance <= minAcceptance)
        {
            SceneManager.LoadScene("FinalRuim");
        }
    }
}