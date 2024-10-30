using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnStation : MonoBehaviour
{
    public string StationName;

    [SerializeField] private Transform SpawnPoint;
    private void Start()
    {
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
        if (FindFirstObjectByType<SaveLoadJson>().GiveSaveData().CurrentStation == StationName)
        {
            Transform Player = FindFirstObjectByType<PlayerActions>().GetComponent<Transform>();

            Player.position = SpawnPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindFirstObjectByType<QuestManager>().SaveAll();
            FindFirstObjectByType<PlayerStats>().SaveStats();
            FindFirstObjectByType<Inventory>().SaveInvetory();
            FindFirstObjectByType<SaveLoadJson>().GiveSaveData().CurrentStation = StationName;
            FindFirstObjectByType<SaveLoadJson>().GiveSaveData().LastLoadedScene = FindFirstObjectByType<SceneManagment>().CurrentSceneName;
            FindFirstObjectByType<SaveLoadJson>().SaveGame();
        }
    }
}
