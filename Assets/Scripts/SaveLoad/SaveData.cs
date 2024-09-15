using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class SaveData
{
    //Fossil Information
    public bool[] FossilDiscoveredStatus = new bool[24]; // 0 to 23

    //Save Station Information
    public string LastLoadedScene; // Last Scene Played In game
    public string CurrentStation; // Last save Station Visited

    //Inventory Informaiton
    public bool[] InventorySlotsFull = new bool[4]; //If Slot has something in it
    public int[] FossilInSlot = new int[4]; // num between 1 and 24, 0 is None
    public bool BigMonsterSlot; // Does Player have a big monster
    public int BigMonster; // what the big monster is
    public bool SmallMonsterSlot; // Does Player have a small monster
    public int SmallMonster; // what the small monster is

    //Set Combat Information
    public bool[] PlanetAreaSetCombats = new bool[1];// num of combats in area
}
