using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StartButtonText;

    private void OnGUI()
    {
        if(File.Exists(FindObjectOfType<SaveLoadJson>().SaveFilePath))
        {
            StartButtonText.text = "Continue";
        }
        else
        {
            StartButtonText.text = "New Game";
        }
    }

    public void StartGame()
    {
        if (File.Exists(FindObjectOfType<SaveLoadJson>().SaveFilePath))
        {
            FindObjectOfType<SceneManagment>().ChangeScene(FindObjectOfType<SaveLoadJson>().GiveSaveData().LastLoadedScene);
        }
        else
        {
            FindObjectOfType<SceneManagment>().ChangeScene("SpaceHQ");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
