using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class Life : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int maxHearts = 5;
    public int currentHearts;

    [Header("Configurações de UI")]
    public Image[] heartImages; // Arraste os objetos de Imagem dos corações aqui
    public Sprite fullHeart;    // Sprite do coração cheio
    public Sprite emptyHeart;   // Sprite do coração vazio/ tirar a imagem do coração 


    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
    }

    // Método para receber dano
    public void TakeDamage(int damage)
    {
        currentHearts -= damage;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            Die();
        }
    }

    // Atualiza visualmente os corações
    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHearts)
            {
                heartImages[i].sprite = fullHeart;
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
                // Ou apenas heartImages[i].enabled = false; se quiser que sumam, assim para que fiquem vazios heartImages[i].sprite = emptyHeart;
            }
        }
    }

    private void Die()
    {
        Debug.Log("Você morreu!");

        // Aplica penalidade de morte
        if (Points_Maneger.Instance != null)
        {
            Points_Maneger.Instance.PlayerMorreu();
        }

        SceneManager.LoadScene("SampleScene");
    }
}