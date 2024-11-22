using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class ReturnStation : MonoBehaviour
{
    public string StationName;

    [SerializeField] private Transform SpawnPoint;

    private QuestManager questManager;
    private PlayerStats playerStats;
    private Inventory inventory;
    private SaveLoadJson saveLoad;
    private SceneManagment sceneManagement;

    private void Start()
    {
        questManager = FindFirstObjectByType<QuestManager>();
        playerStats = FindFirstObjectByType<PlayerStats>();
        inventory = FindFirstObjectByType<Inventory>();
        saveLoad = FindFirstObjectByType<SaveLoadJson>();
        sceneManagement = FindFirstObjectByType<SceneManagment>();

        SetPlayerPos();
    }

    private void OnEnable()
    {
        SceneManagment.NewSceneLoaded += SetPlayerPos;
        RespawnScript.RespawnCall += SetPlayerPos;
    }

    private void OnDisable()
    {
        SceneManagment.NewSceneLoaded -= SetPlayerPos;
        RespawnScript.RespawnCall -= SetPlayerPos;
    }

    public void SetPlayerPos()
    {
        if (saveLoad.GiveSaveData().CurrentStation == StationName)
        {
            Transform Player = FindFirstObjectByType<PlayerActions>().GetComponent<Transform>();
            Player.position = SpawnPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerActions>() != null)
        {
            try
            {
                Debug.Log("Saving quests...");
                questManager?.SaveAll();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving quests: " + e.Message);
            }

            try
            {
                Debug.Log("Saving player stats...");
                playerStats?.SaveStats();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving player stats: " + e.Message);
            }

            try
            {
                Debug.Log("Saving inventory...");
                inventory?.SaveInvetory();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving inventory: " + e.Message);
            }

            try
            {
                Debug.Log($"Updating CurrentStation from {saveLoad.GiveSaveData().CurrentStation} to {StationName}");
                saveLoad.GiveSaveData().CurrentStation = StationName;

                Debug.Log($"Updating LastLoadedScene from {saveLoad.GiveSaveData().LastLoadedScene} to {sceneManagement.CurrentSceneName}");
                saveLoad.GiveSaveData().LastLoadedScene = sceneManagement.CurrentSceneName;

                Debug.Log("QuestSaveStates before SaveGame in ReturnStation: " + JsonUtility.ToJson(saveLoad.GiveSaveData().QuestSaveStates));

                Debug.Log("Saving game...");
                saveLoad.SaveGame();

                Debug.Log("Save process complete.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error finalizing save: " + e.Message);
            }
        }
    }
}
