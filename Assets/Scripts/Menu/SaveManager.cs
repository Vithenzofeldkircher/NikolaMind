using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private ISaveSystem saveSystem;
    private SaveData pendingLoadData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Instancia a persistência em JSON
        saveSystem = new JsonSaveSystem();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SaveGame(int slotIndex)
    {
        Debug.Log($"[SaveManager] Salvando jogo no Slot {slotIndex}...");

        SaveData data = CurrentGameStateToSaveData();
        if (data != null)
        {
            saveSystem.Save(data, slotIndex);
            Debug.Log($"[SaveManager] Sucesso! Posição Salva: ({data.playerPosX}, {data.playerPosY}, {data.playerPosZ})");
        }
    }

    public void LoadGame(int slotIndex)
    {
        Debug.Log($"[SaveManager] Carregando Slot {slotIndex}...");

        SaveData data = saveSystem.Load(slotIndex);
        if (data == null)
        {
            Debug.LogWarning($"[SaveManager] Nenhum arquivo de save foi encontrado no Slot {slotIndex}.");
            return;
        }

        pendingLoadData = data;

        // Se o save for na mesma cena atual
        if (SceneManager.GetActiveScene().name == data.sceneName)
        {
            StartCoroutine(ApplyDataNextFrame(data));
        }
        else
        {
            // Se for em outra cena, carrega a cena (o callback OnSceneLoaded vai aplicar os dados)
            SceneManager.LoadScene(data.sceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pendingLoadData != null)
        {
            StartCoroutine(ApplyDataNextFrame(pendingLoadData));
        }
    }

    private IEnumerator ApplyDataNextFrame(SaveData data)
    {
        // Garante que o tempo do jogo não está congelado
        Time.timeScale = 1f;

        // Aguarda um frame para a cena/física estabilizarem
        yield return new WaitForEndOfFrame();

        ApplySaveDataToGame(data);
        pendingLoadData = null;
    }

    private SaveData CurrentGameStateToSaveData()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[SaveManager] ERRO: Nenhum GameObject com a Tag 'Player' foi encontrado!");
            return null;
        }

        SaveData data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosX = player.transform.position.x,
            playerPosY = player.transform.position.y,
            playerPosZ = player.transform.position.z,
            playerRotX = player.transform.eulerAngles.x,
            playerRotY = player.transform.eulerAngles.y,
            playerRotZ = player.transform.eulerAngles.z,
            playTime = Time.timeSinceLevelLoad
        };

        return data;
    }

    private void ApplySaveDataToGame(SaveData data)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[SaveManager] ERRO: Não foi possível aplicar o Load. Player não encontrado!");
            return;
        }

        // Obtém componentes de física para desativar temporariamente
        Rigidbody rb3D = player.GetComponent<Rigidbody>();
        Rigidbody2D rb2D = player.GetComponent<Rigidbody2D>();
        CharacterController cc = player.GetComponent<CharacterController>();

        // Desativa física para evitar re-colisão/sobrescrita de posição
        if (rb3D != null) rb3D.isKinematic = true;
        if (rb2D != null) rb2D.simulated = false;
        if (cc != null) cc.enabled = false;

        // Aplica a nova posição e rotação
        Vector3 targetPosition = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
        Vector3 targetRotation = new Vector3(data.playerRotX, data.playerRotY, data.playerRotZ);

        player.transform.position = targetPosition;
        player.transform.eulerAngles = targetRotation;

        // Reativa a física
        if (rb3D != null) rb3D.isKinematic = false;
        if (rb2D != null) rb2D.simulated = true;
        if (cc != null) cc.enabled = true;

        Debug.Log($"[SaveManager] Player movido com sucesso para: {targetPosition}");
    }
}