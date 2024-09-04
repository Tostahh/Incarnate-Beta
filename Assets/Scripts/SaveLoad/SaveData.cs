using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool[] FossilDiscoveredStatus = new bool[24];


    public int LoadedScene;
    public Vector3 PlayerPosition;

    public bool[] PlayerInventorySlotsFull = new bool[4];
    public int[] FossilInInventory = new int[4];



}
