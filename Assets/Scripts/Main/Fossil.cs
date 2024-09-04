using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fossil : MonoBehaviour
{
    [Header("FossilInformation")]
    public bool Discovered;
    public string Name;
    public string Description;
    public GameObject SongPrefab;
    public GameObject CometMonsterPrafab; // Temp, will be unlocked in a menu first then added to party in full version

    private bool Distroyed;
    private void Awake()
    {
        DontDestroyOnLoad(this);
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
        Discovered = true;
    }
    public void SongLost()
    {
        Distroyed = true;
    }
    public string GiveName()
    {
        if(Discovered)
        {
            return name;
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
        if(Discovered)
        {
            Instantiate(CometMonsterPrafab, FindObjectOfType<Inventory>().gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (Distroyed)
        {
            Destroy(gameObject);
        }
    }
}
