using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    
    [Header("Painel de Configurações")]
    
    public GameObject painelConfiguraçoes;
    
  
    
    public void AbrirCena(string nomeDaaCena)  //Ir para a ouitra cena
    {
        SceneManager.LoadScene(nomeDaaCena);
    }

    public void AbrirConfiguracoes() //Abrir painel
    {
        painelConfiguraçoes.SetActive(true);
    }
    public void FecharConfiguracoes()
    {
        painelConfiguraçoes.SetActive(false);
    }

    
    public void SairDoJogo() //Sair do jogo
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
