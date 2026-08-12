using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static PauseMenu instance;

    public GameObject pausePanel;

    public static bool isGamePaused = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Voltar();
            }
            else
            {
                AbrirPause();
            }
        }
    }

    public void AbrirPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Voltar()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;

        Destroy(gameObject);

        SceneManager.LoadScene("Tela Inicial");
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}