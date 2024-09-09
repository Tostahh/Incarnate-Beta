using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AreaSelect : Interactable
{
    [SerializeField] private GameObject AreaSelectUI;
    [SerializeField] private TextMeshProUGUI[] AreaNames;

    [SerializeField] private string[] SceneNames;

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
                    AreaNames[i].text = SceneNames[i];
                }
            }
        }
    }

    public void ChangeToScene(int i)
    {
        FindObjectOfType<SceneManagment>().ChangeScene(SceneNames[i]);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            AreaSelectUI.SetActive(false);
        }
    }
}
