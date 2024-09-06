using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Cleaning Station information
    public bool[] FossilDiscoveredStatus = new bool[24];

    //Save Station Information
    public int LastLoadedScene;
    public string CurrentStation;
}
