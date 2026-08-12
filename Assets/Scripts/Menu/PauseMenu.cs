using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;

    [Header("Input Settings")]
    [SerializeField] private string cancelInputButton = "Cancel"; // Mapeado para 'ESC' e botões de Pause

    public static bool isGamePaused = false;

    #region Unity Lifecycle

    private void Awake()
    {
        SetupSingleton();
    }

    private void Update()
    {
        HandlePauseInput();
    }


    private void HandlePauseInput()
    {
        // Alterado para GetButtonDown para suporte flexível (Teclado/Controle)
        if (Input.GetButtonDown(cancelInputButton))
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

    private void SetupSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Public UI Controls

    public void AbrirPause()
    {
        SetPauseState(true);
    }

    public void Voltar()
    {
        SetPauseState(false);
    }

    private void SetPauseState(bool isPaused)
    {
        isGamePaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
    }


    public void Button_SaveSlot(int slotIndex)
    {
        Debug.Log($"[PauseMenu] Botão Save Clicado no Slot {slotIndex}");

        SaveManager manager = GetSaveManager();
        if (manager != null)
        {
            manager.SaveGame(slotIndex);
        }
    }

    public void Button_LoadSlot(int slotIndex)
    {
        Debug.Log($"[PauseMenu] Botão Load Clicado no Slot {slotIndex}");

        // Fecha o painel de pause e restaura o tempo do jogo
        Voltar();

        SaveManager manager = GetSaveManager();
        if (manager != null)
        {
            manager.LoadGame(slotIndex);
        }
    }

    private SaveManager GetSaveManager()
    {
        SaveManager manager = SaveManager.Instance;
        if (manager == null)
        {
            manager = FindFirstObjectByType<SaveManager>();
        }

        if (manager == null)
        {
            Debug.LogError("[PauseMenu] ERRO: Nenhum SaveManager foi encontrado na cena!");
        }

        return manager;
    }

    #endregion

    #region Navigation Commands

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;

        // Limpa este Canvas do DontDestroyOnLoad ao retornar ao menu principal
        Destroy(gameObject);
        SceneManager.LoadScene("Tela Inicial");
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    #endregion
}