using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private ISaveSystem saveSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Injeção da implementação de save
        saveSystem = new JsonSaveSystem();
    }

    public void SaveGame(int slotIndex)
    {
        SaveData data = CurrentGameStateToSaveData();
        saveSystem.Save(data, slotIndex);
    }

    public void LoadGame(int slotIndex)
    {
        SaveData data = saveSystem.Load(slotIndex);
        if (data != null)
        {
            ApplySaveDataToGame(data);
        }
    }

    private SaveData CurrentGameStateToSaveData()
    {
        SaveData data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;

        // Exemplo de coleta de dados do Player (Busque a referência do seu Player conforme a arquitetura do seu projeto)
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            Vector3 rot = player.transform.eulerAngles;

            data.playerPosX = pos.x;
            data.playerPosY = pos.y;
            data.playerPosZ = pos.z;

            data.playerRotX = rot.x;
            data.playerRotY = rot.y;
            data.playerRotZ = rot.z;
        }

        data.playTime = Time.timeSinceLevelLoad;
        // data.playerHealth = PlayerHealth.Instance.CurrentHealth; // Exemplo

        return data;
    }

    private void ApplySaveDataToGame(SaveData data)
    {
        // Se o save for de outra cena, recarrega a cena primeiro
        if (SceneManager.GetActiveScene().name != data.sceneName)
        {
            SceneManager.LoadScene(data.sceneName);
            // Nota: Em um projeto real, você usaria um callback do SceneManager.sceneLoaded para reposicionar o player após carregar a cena.
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            player.transform.eulerAngles = new Vector3(data.playerRotX, data.playerRotY, data.playerRotZ);
        }
    }
}