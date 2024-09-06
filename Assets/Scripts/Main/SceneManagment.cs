using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneManagment : MonoBehaviour
{
    public static Action NewSceneLoaded = delegate { };

    public int CurrentSceneNumb;

    private void Awake()
    {
        CurrentSceneNumb = SceneManager.GetActiveScene().buildIndex;
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
        CurrentSceneNumb = SceneManager.GetActiveScene().buildIndex;
        // In transition

        NewSceneLoaded();
    }
}
