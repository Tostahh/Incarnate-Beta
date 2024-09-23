using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private bool Player;
    [SerializeField] private bool Inverse;

    [SerializeField] private int TutorialNumb;

    private bool Done;

    private void OnEnable()
    {
        Done = FindObjectOfType<SaveLoadJson>().GiveSaveData().TutorialsDone[TutorialNumb];
        if(Done)
        {
            GetComponentInChildren<Collider>().enabled = false;
            if(Inverse)
            {
                UI.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Inverse)
        {
            if (!Done)
            {
                if (Player)
                {
                    if (other.gameObject.CompareTag("Player"))
                    {
                        UI.SetActive(true);
                    }
                }
                else
                {
                    UI.SetActive(true);
                }
            }
        }
        else
        {
            if (!Done)
            {
                if (Player)
                {
                    if (other.gameObject.CompareTag("Player"))
                    {
                        UI.SetActive(false);
                    }
                }
                else
                {
                    UI.SetActive(false);
                }
                Done = true;
                GetComponentInChildren<Collider>().enabled = false;
                FindObjectOfType<SaveLoadJson>().GiveSaveData().TutorialsDone[TutorialNumb] = Done;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!Inverse)
        {
            if (!Done)
            {
                if (Player)
                {
                    if (other.gameObject.CompareTag("Player"))
                    {
                        UI.SetActive(false);
                    }
                }
                else
                {
                    UI.SetActive(false);
                }
                Done = true;
                GetComponentInChildren<Collider>().enabled = false;
                FindObjectOfType<SaveLoadJson>().GiveSaveData().TutorialsDone[TutorialNumb] = Done;
            }
        }
    }
}
