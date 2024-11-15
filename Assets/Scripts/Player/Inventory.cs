using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Starbits; // Currency, SB = abrv

    public int PearlStone; // 5 SB T1 Material
    public int RootStem; // 25 SB T2 Material
    public int DeepfrostOre; // 100 SB T3 Material

    public int DarkDisasterKey; // requires 5 to beat the game

    public int FossilSlots = 4;

    public bool[] SlotIsFull;
    public GameObject[] Slots;

    public bool BigMonsterSlotFull;
    public GameObject BigMonsterSlot;

    public bool SmallMonsterSlotFull;
    public GameObject SmallMonsterSlot;

    public GameObject SwapSlotItem;

    [SerializeField] private GameObject[] FossilPrefabs;
    [SerializeField] private GameObject[] MonsterPrefabs;
    public bool InvetoryIsFull()
    {
        bool IsFull = true;

        for (int i = 0; i < SlotIsFull.Length; i++)
        {
            if (SlotIsFull[i] == false)
            {
                IsFull = false;
            }
        }

        return IsFull;
    }
    public void SaveInvetory()
    {
        FindObjectOfType<SaveLoadJson>().GiveSaveData().StarBits = Starbits;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().PearlStone = PearlStone;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().RootStem = RootStem;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().DeepfrostOre = DeepfrostOre;
        FindObjectOfType<SaveLoadJson>().GiveSaveData().DarkDisasterKey = DarkDisasterKey;

        FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilSlots = FossilSlots;

        FindObjectOfType<SaveLoadJson>().GiveSaveData().InventorySlotsFull = new bool[FossilSlots];
        FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilInSlot = new int[FossilSlots];

        for (int i = 0; i < FindObjectOfType<SaveLoadJson>().GiveSaveData().InventorySlotsFull.Length; i++)
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().InventorySlotsFull[i] = SlotIsFull[i];
            if(SlotIsFull[i])
            {
                FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilInSlot[i] = Slots[i].GetComponent<Fossil>().FossilNum + 1;
            }
            else
            {
                FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilInSlot[i] = 0;
            }
        }

        FindObjectOfType<SaveLoadJson>().GiveSaveData().BigMonsterSlot = BigMonsterSlotFull;
        if (BigMonsterSlotFull)
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().BigMonster = BigMonsterSlot.GetComponent<BattleForm>().MonsterNum;
        }
        else
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster = 0;
        }

        FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonsterSlot = SmallMonsterSlotFull;
        if (SmallMonsterSlotFull)
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster = SmallMonsterSlot.GetComponent<SupportForm>().MonsterNum;
        }
        else
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster = 0;
        }
    }
    public void LoadInventory()
    {
        Starbits = FindObjectOfType<SaveLoadJson>().GiveSaveData().StarBits;
        PearlStone = FindObjectOfType<SaveLoadJson>().GiveSaveData().PearlStone;
        RootStem = FindObjectOfType<SaveLoadJson>().GiveSaveData().RootStem;
        DeepfrostOre = FindObjectOfType<SaveLoadJson>().GiveSaveData().DeepfrostOre;
        DarkDisasterKey = FindObjectOfType<SaveLoadJson>().GiveSaveData().DarkDisasterKey;

        Starbits = FindObjectOfType<SaveLoadJson>().GiveSaveData().StarBits;

        FossilSlots = FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilSlots;


        SlotIsFull = new bool[FossilSlots];
        Slots = new GameObject[FossilSlots];

        for (int i = 0; i < FindObjectOfType<SaveLoadJson>().GiveSaveData().InventorySlotsFull.Length; i++)
        {
            SlotIsFull[i] = FindObjectOfType<SaveLoadJson>().GiveSaveData().InventorySlotsFull[i];
            if (SlotIsFull[i])
            {
                Slots[i] = Instantiate(FossilPrefabs[FindObjectOfType<SaveLoadJson>().GiveSaveData().FossilInSlot[i]-1]);
            }
        }

        if(FindObjectOfType<SaveLoadJson>().GiveSaveData().BigMonsterSlot == true)
        {
            BigMonsterSlot = Instantiate(MonsterPrefabs[FindObjectOfType<SaveLoadJson>().GiveSaveData().BigMonster-1]);
        }

        if (FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonsterSlot == true)
        {
            SmallMonsterSlot = Instantiate(MonsterPrefabs[FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster-1]);
        }
    }

    public void UpgradeInventory()
    {
        bool[] tmp = new bool[SlotIsFull.Length + 4];
        GameObject[] ftmp = new GameObject[Slots.Length + 4];

        for (int i = 0; i < SlotIsFull.Length; i++)
        {
            tmp[i] = SlotIsFull[i];
            if(tmp[i] == true)
            {
                ftmp[i] = Slots[i];
            }
        }

        SlotIsFull = new bool[tmp.Length];
        Slots = new GameObject[ftmp.Length];

        for (int i = 0; i < tmp.Length; i++)
        {
            SlotIsFull[i] = tmp[i];
            if (SlotIsFull[i] == true)
            {
                Slots[i] = ftmp[i];
            }
        }

        FossilSlots = SlotIsFull.Length;
    }
}
