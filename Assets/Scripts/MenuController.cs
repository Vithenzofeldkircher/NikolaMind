using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Painel de Configurações")]
    public GameObject painelConfiguraçoes;
    
    //Ir para a ouitra cena
    public void AbrirCena(string nomeDaaCena)
    {
        SceneManager.LoadScene(nomeDaaCena);
    }

    //Abrir painel
    public void AbrirConfiguracoes()
    {
        painelConfiguraçoes.SetActive(true);
    }
    public void FecharConfiguracoes()
    {
        painelConfiguraçoes.SetActive(false);
    }

    //Sair do jogo
    public void SairDoJogo()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
