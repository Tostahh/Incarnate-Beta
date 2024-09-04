using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadJson : MonoBehaviour // Learning Save Data
{
    public SaveData SaveDataInput;

    string SaveFilePath;

    private void Awake()
    {
        SaveDataInput = new SaveData();
    }
    private void Start()
    {
        SaveFilePath = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(SaveFilePath))
        {
            LoadGame();
        }
    }

    public SaveData GiveSaveData()
    {
        return SaveDataInput;
    }

    public void SaveGame()
    {
        string SavePlayerData = JsonUtility.ToJson(SaveDataInput);
        File.WriteAllText(SaveFilePath, SavePlayerData);

        Debug.Log("Save file created at: " + SaveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(SaveFilePath))
        {
            string loadPlayerData = File.ReadAllText(SaveFilePath);
            SaveDataInput = JsonUtility.FromJson<SaveData>(loadPlayerData);

            Debug.Log("Load game complete!");
        }
        else
        {
            Debug.Log("There is no save files to load!");
        }
    }
    public void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);

            Debug.Log("Save file deleted!");
        }
        else
        {
            Debug.Log("There is nothing to delete!");
        }
    }

}
