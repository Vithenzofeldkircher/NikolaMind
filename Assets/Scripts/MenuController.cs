using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Nomes das Cenas")]
    [Tooltip("Nome da cena principal de jogabilidade.")]
    public string nomeCenaPrincipal;

    [Tooltip("Nome da cena de configurações e opções.")]
    public string nomeCenaOpcoes;

    [Tooltip("Nome da cena que exibe os créditos do jogo.")]
    public string nomeCenaCreditos;

    public void CarregarCenaPrincipal()
    {
        if (!string.IsNullOrEmpty(nomeCenaPrincipal))
        {
            SceneManager.LoadScene(nomeCenaPrincipal);
        }
        else
        {
            Debug.LogError("O nome da cena Principal não foi definido no Inspector!");
        }
    }

    public void CarregarCenaOpcoes()
    {
        if (!string.IsNullOrEmpty(nomeCenaOpcoes))
        {
            SceneManager.LoadScene(nomeCenaOpcoes);
        }
        else
        {
            Debug.LogError("O nome da cena de Opções não foi definido no Inspector!");
        }
    }

    public void CarregarCenaCreditos()
    {
        if (!string.IsNullOrEmpty(nomeCenaCreditos))
        {
            SceneManager.LoadScene(nomeCenaCreditos);
        }
        else
        {
            Debug.LogError("O nome da cena de Créditos não foi definido no Inspector!");
        }
    }

    public void SairDoJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}