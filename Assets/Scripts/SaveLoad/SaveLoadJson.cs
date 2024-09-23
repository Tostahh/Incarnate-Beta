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
        for (int i = 0; i < SaveDataInput.FossilDiscoveredStatus.Length; i++)
        {
            SaveDataInput.FossilDiscoveredStatus[i] = false;
        }

        for (int i = 0; i < SaveDataInput.TutorialsDone.Length; i++)
        {
            SaveDataInput.TutorialsDone[i] = false;
        }

        SaveDataInput.LastLoadedScene = "PlanetArea";
        SaveDataInput.CurrentStation = "StartStation";

        for (int i = 0; i < SaveDataInput.InventorySlotsFull.Length; i++)
        {
            SaveDataInput.InventorySlotsFull[i] = false;
            SaveDataInput.FossilInSlot[i] = 0;
        }

        SaveDataInput.BigMonsterSlot = false;
        SaveDataInput.BigMonster = 0;
        SaveDataInput.SmallMonsterSlot = false;
        SaveDataInput.SmallMonster = 0;

        for (int i = 0; i < SaveDataInput.PlanetAreaSetCombats.Length; i++) // Do for each Area Array
        {
            SaveDataInput.PlanetAreaSetCombats[i] = false;
        }

        SaveGame();
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
