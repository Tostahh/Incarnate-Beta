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
    }
    private void OnDisable()
    {
        SceneManagment.NewSceneLoaded -= SetPlayerPos;
    }
    public void SetPlayerPos()
    {
        if (FindObjectOfType<SaveLoadJson>().GiveSaveData().CurrentStation == StationName)
        {
            Transform Player = FindObjectOfType<PlayerActions>().GetComponent<Transform>();

            Player.position = SpawnPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().CurrentStation = StationName;
            FindObjectOfType<SaveLoadJson>().GiveSaveData().LastLoadedScene = FindObjectOfType<SceneManagment>().CurrentSceneName;
            FindObjectOfType<SaveLoadJson>().SaveGame();
        }
    }
}
