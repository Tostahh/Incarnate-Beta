using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneManagment : MonoBehaviour
{
    public static Action NewSceneLoaded = delegate { };

    private int TempSceneNumb;

    private void Start()
    {
        TempSceneNumb = SceneManager.GetActiveScene().buildIndex;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().LastLoadedScene = TempSceneNumb;
        FindObjectOfType<SaveLoadJson>().SaveGame();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string SceneName)
    {
        // Out Transition

        SceneManager.LoadScene(SceneName);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // In transition

        TempSceneNumb = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(FindObjectOfType<SaveLoadJson>().GiveSaveData().LastLoadedScene);
        FindObjectOfType<SaveLoadJson>().GiveSaveData().LastLoadedScene = TempSceneNumb;
        FindObjectOfType<SaveLoadJson>().SaveGame();

        NewSceneLoaded();
    }
}
