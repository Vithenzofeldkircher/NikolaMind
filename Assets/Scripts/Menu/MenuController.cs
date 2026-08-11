using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string cenaIniciar;

    public GameObject painelConfiguracoes;
    public GameObject painelCreditos;

    public void IniciarJogo()
    {
        SceneManager.LoadScene(cenaIniciar);
    }

    public void AbrirConfiguracoes()
    {
        painelConfiguracoes.SetActive(true);
    }

    public void FecharConfiguracoes()
    {
        painelConfiguracoes.SetActive(false);
    }

    public void AbrirCreditos()
    {
        painelCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        painelCreditos.SetActive(false);
    }

    public void Sair()
    {
        Application.Quit();
    }
}