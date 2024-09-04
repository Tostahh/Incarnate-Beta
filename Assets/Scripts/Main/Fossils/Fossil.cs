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

    SaveData SaveData;
    private void Awake()
    {
        SaveData = FindObjectOfType<SaveLoadJson>().GiveSaveData();

        DontDestroyOnLoad(this);
        if (SaveData.FossilDiscoveredStatus.Length >= FossilNum)
        {
            Discovered = SaveData.FossilDiscoveredStatus[FossilNum];
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
                Instantiate(CometMonsterPrafab, FindObjectOfType<Inventory>().gameObject.transform.position, Quaternion.identity);

                SaveData.FossilDiscoveredStatus[FossilNum] = Discovered;
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
