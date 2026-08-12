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

        SaveManager manager = GetSaveManager();
        if (manager != null)
        {
            // Inicia a transição fluida via Corrotina
            StartCoroutine(LoadSlotRoutine(manager, slotIndex));
        }
    }

    private IEnumerator LoadSlotRoutine(SaveManager manager, int slotIndex)
    {
        // Restaura o tempo do jogo
        Time.timeScale = 1f;

        // Dispara o carregamento no SaveManager
        manager.LoadGame(slotIndex);

        // Aguarda até o final do frame para garantir que a transição e atualização de posição ocorreram
        yield return new WaitForEndOfFrame();

        // Esconde o painel e despausa a flag do menu
        Voltar();
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


    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;

        StartCoroutine(IrParaMenuRoutine("Tela Inicial"));
    }

    private IEnumerator IrParaMenuRoutine(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        // Aguarda até que a cena ativa seja efetivamente a 'Tela Inicial'
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name.Equals(sceneName));

        // Esconde o objeto e o destrói para liberar memória
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}