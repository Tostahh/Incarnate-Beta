using UnityEngine;
using System;

[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    public bool Axe;
    public bool Sling;

    public int SpearDmg;
    public int AxeDmg;
    public int SlingDmg;

    public int SearchRange;

    public bool Lantern;

    public bool Mount; // set up elsewhere
    public bool DoubleJump; // ^^^

    private void Start()
    {
        LoadStats();
    }
    public void SaveStats()
    {
        FindObjectOfType<SaveLoadJson>().GiveSaveData().SpearDmg = SpearDmg;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().AxeDmg = AxeDmg;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().SlingDmg = SlingDmg;

        FindObjectOfType<SaveLoadJson>().GiveSaveData().RadarRange = SearchRange;

        FindObjectOfType<SaveLoadJson>().GiveSaveData().Lantern = Lantern;

    }
    public void LoadStats()
    {
        SpearDmg = FindObjectOfType<SaveLoadJson>().GiveSaveData().SpearDmg;
        AxeDmg = FindObjectOfType<SaveLoadJson>().GiveSaveData().AxeDmg;
        SlingDmg = FindObjectOfType<SaveLoadJson>().GiveSaveData().SlingDmg;

        SearchRange = FindObjectOfType<SaveLoadJson>().GiveSaveData().RadarRange;

        Lantern = FindObjectOfType<SaveLoadJson>().GiveSaveData().Lantern;
    }
}
