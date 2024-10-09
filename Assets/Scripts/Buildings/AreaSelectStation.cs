using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AreaSelectStation : Interactable
{
    [SerializeField] private GameObject AreaSelectUI;

    [SerializeField] private TextMeshProUGUI[] AreaNames;
    [SerializeField] private string[] SceneNames;

    [SerializeField] private string[] StationNames;

    public override void Interact(InputAction.CallbackContext Shift)
    {
        if(PlayerInRange)
        {
            if (AreaSelectUI.activeSelf == true)
            {
                AreaSelectUI.SetActive(false);
            }
            else
            {
                AreaSelectUI.SetActive(true);
                for (int i = 0; i < AreaNames.Length; i++)
                {
                    if (StationNames.Length > 0)
                    {
                        AreaNames[i].text = SceneNames[0] + " " + StationNames[i];
                    }
                    else
                    {
                        AreaNames[i].text = SceneNames[0];
                    }
                }
            }
        }
    }

    public void ChangeToScene(int i)
    {
        FindObjectOfType<SaveLoadJson>().GiveSaveData().LastLoadedScene = SceneNames[i];
        FindObjectOfType<SceneManagment>().ChangeScene(SceneNames[i]);
    }
    
    public void ChangeCurrentStation(int i)
    {
        FindObjectOfType<SaveLoadJson>().GiveSaveData().CurrentStation = StationNames[i];
    }
}
