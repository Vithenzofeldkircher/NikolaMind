using System.IO;
using UnityEngine;

public interface ISaveSystem
{
    void Save(SaveData data, int slotIndex);
    SaveData Load(int slotIndex);
    bool SaveExists(int slotIndex);
}

public class JsonSaveSystem : ISaveSystem
{
    private string GetFilePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
    }

    public void Save(SaveData data, int slotIndex)
    {
        string path = GetFilePath(slotIndex);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Jogo salvo com sucesso no Slot {slotIndex} em: {path}");
    }

    public SaveData Load(int slotIndex)
    {
        string path = GetFilePath(slotIndex);

        if (!SaveExists(slotIndex))
        {
            Debug.LogWarning($"Nenhum save encontrado no Slot {slotIndex}.");
            return null;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log($"Jogo carregado com sucesso do Slot {slotIndex}.");
        return data;
    }

    public bool SaveExists(int slotIndex)
    {
        return File.Exists(GetFilePath(slotIndex));
    }
}