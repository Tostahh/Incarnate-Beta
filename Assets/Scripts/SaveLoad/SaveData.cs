using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool[] FossilDiscoveredStatus = new bool[24];

    public int LastLoadedScene = 0;

    public string CurrentStation;
}
