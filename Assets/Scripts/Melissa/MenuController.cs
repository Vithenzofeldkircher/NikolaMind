using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Painel de Configurações")]
    public GameObject painelConfiguracoes;
    public CreditScroll creditScroll;

    [Header("Painel de Créditos")]
    public GameObject painelCreditos;

    public void AbrirCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    // Configurações
    public void AbrirConfiguracoes()
    {
        painelConfiguracoes.SetActive(true);
    }

    public void FecharConfiguracoes()
    {
        painelConfiguracoes.SetActive(false);
    }

    // Créditos
    public void AbrirCreditos()
{
    painelCreditos.SetActive(true);
    creditScroll.IniciarCreditos();
}

public void FecharCreditos()
{
    creditScroll.PararCreditos();
    painelCreditos.SetActive(false);
}

    public void SairDoJogo()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}