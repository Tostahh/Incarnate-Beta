using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] SlotIsFull;
    public GameObject[] Slots;

    public bool BigMonsterSlotFull;
    public GameObject BigMonsterSlot;

    public bool SmallMonsterSlotFull;
    public GameObject SmallMonsterSlot;

    [SerializeField] private GameObject[] FossilPrefabs;
    [SerializeField] private GameObject[] MonsterPrefabs;
    public void SaveInvetory()
    {
        for (int i = 0; i < SlotIsFull.Length; i++)
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
            FindObjectOfType<SaveLoadJson>().GiveSaveData().BigMonster = BigMonsterSlot.GetComponent<BigMonster>().MonsterNum;
        }
        else
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster = 0;
        }

        FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonsterSlot = SmallMonsterSlotFull;
        if (SmallMonsterSlotFull)
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster = SmallMonsterSlot.GetComponent<SmallMonster>().MonsterNum;
        }
        else
        {
            FindObjectOfType<SaveLoadJson>().GiveSaveData().SmallMonster = 0;
        }
    }

    public void LoadInventory()
    {
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
}
