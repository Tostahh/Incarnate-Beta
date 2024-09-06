using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fossil : MonoBehaviour
{

    [Header("FossilInformation")]
    public int FossilNum;
    public bool Discovered;
    public string Name;
    public string Description;
    public GameObject SongPrefab;
    public GameObject CometMonsterPrafab; // Temp, will be unlocked in a menu first then added to party in full version

    private bool Cleaned;
    private bool Destroyed;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilDiscoveredStatus.Length >= FossilNum)
        {
            Debug.Log(FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilDiscoveredStatus[FossilNum]);
            Discovered = FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilDiscoveredStatus[FossilNum];
        }
    }
    private void OnEnable()
    {
        SceneManagment.NewSceneLoaded += Done;
    }
    private void OnDisable()
    {
        SceneManagment.NewSceneLoaded -= Done;
    }
    public void SongCompleted()
    {
        Cleaned = true;
        if (!Discovered)
        {
            Discovered = true;
        }
    }
    public void SongLost()
    {
        Destroyed = true;
    }
    public string GiveName()
    {
        if(Discovered)
        {
            return Name;
        }
        else
        {
            return "???";
        }
    }

    public string GiveDescription()
    {
        if(Discovered)
        {
            return Description;
        }
        else
        {
            return "There is something hidden inside!";
        }
    }

    public void Done()
    {
        if (Cleaned)
        {
            if (Discovered)
            {
                FindObjectOfType<Inventory>().gameObject.transform.parent.gameObject.SetActive(true);
                Instantiate(CometMonsterPrafab, FindObjectOfType<Inventory>().gameObject.transform.position, Quaternion.identity);

                FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilDiscoveredStatus[FossilNum] = Discovered;
                FindObjectOfType<SaveLoadJson>().SaveGame();

                Destroy(gameObject);
            }

            if (Destroyed)
            {
                Destroy(gameObject);
            }
        }
    }
}
