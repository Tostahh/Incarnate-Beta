using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadJson : MonoBehaviour // Learning Save Data
{
    private static SaveData SaveDataInput;

    public string SaveFilePath;

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

    public void NewGame()
    {
        SaveData SD = new SaveData();

        for (int i = 0; i < SD.FossilDiscoveredStatus.Length; i++)
        {
            SD.FossilDiscoveredStatus[i] = false;
        }
        SD.LastLoadedScene = "SpaceHQ";
        SD.CurrentStation = "SpaceHQ";
        for (int i = 0; i < SD.InventorySlotsFull.Length; i++)
        {
            SD.InventorySlotsFull[i] = false;
            SD.FossilInSlot[i] = 0;
        }
        SD.BigMonsterSlot = false;
        SD.BigMonster = 0;
        SD.SmallMonsterSlot = false;
        SD.SmallMonster = 0;

        string SDString = JsonUtility.ToJson(SD);
        string SavePlayerData = JsonUtility.ToJson(SaveDataInput);
        File.WriteAllText(SaveFilePath, SDString);
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
