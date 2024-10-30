using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    //Fossil Information
    public bool[] FossilDiscoveredStatus = new bool[24]; // 0 to 23

    //Tutorial Information
    public bool[] TutorialsDone = new bool[10]; // number of pop up texts temp

    //Save Station Information
    public string LastLoadedScene; // Last Scene Played In game
    public string CurrentStation; // Last save Station Visited

    //Inventory Informaiton
    public int StarBits;
    public int PearlStone;
    public int RootStem;
    public int DeepfrostOre;
    public int DarkDisasterKey;

    public bool[] InventorySlotsFull = new bool[4]; //If Slot has something in it
    public int[] FossilInSlot = new int[4]; // num between 1 and 24, 0 is None
    public bool BigMonsterSlot; // Does Player have a big monster
    public int BigMonster; // what the big monster is
    public bool SmallMonsterSlot; // Does Player have a small monster
    public int SmallMonster; // what the small monster is

    //Upgrade Informtion & how to Aquire them
    //Weapons
    public int SpearDmg; // Market Purchase
    public bool Axe; // Exploration Unlock
    public int AxeDmg; // Market Purchase
    public bool Sling; // Exploration Unlock
    public int SlingDmg; // Market Purchase
    //FossilGear
    public int FossilSlots; // Market Purchase
    public int RadarRange; // Market Purchase
    //ExplorationGear
    public bool Mount; // Exploration Unlock
    public bool DoubleJump; // Exploraton Unlock
    public bool Lantern; // Market Purchase

    //Set Combat Information
    public CombatInfo[] SetCombats = new CombatInfo[1]; // Combats by ID and Status
    public Dictionary<string, string> QuestSaveStates = new Dictionary<string, string>(); // Quests states

}
