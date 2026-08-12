using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;

    public static bool isGamePaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
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
        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Voltar()
    {
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        isGamePaused = false;
    }

    // --- Métodos para vincular aos Botões de Save/Load da UI ---

    public void Button_SaveSlot(int slotIndex)
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(slotIndex);
        }
    }

    public void Button_LoadSlot(int slotIndex)
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadGame(slotIndex);
            Voltar(); // Despausa o jogo ao carregar
        }
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