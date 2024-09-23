using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject PopUpUI;

    [SerializeField] private TextMeshProUGUI[] FossilSlots;
    [SerializeField] private TextMeshProUGUI SmallMonsterSlot;
    [SerializeField] private TextMeshProUGUI BigMonsterSlot;

    private Inventory inventory;

    private PlayerControls PC;

    private void Awake()
    {
        PC = new PlayerControls();
        PC.Enable();
    }

    private void OnEnable()
    {
        inventory = GetComponent<Inventory>();

        for (int i = 0; i < FossilSlots.Length; i++)
        {
            if (inventory.SlotIsFull[i])
            {
                FossilSlots[i].text = inventory.Slots[i].gameObject.GetComponent<Fossil>().GiveName();
            }
            else
            {
                FossilSlots[i].text = "Empty";
            }
        }

        if (inventory.BigMonsterSlotFull)
        {
            BigMonsterSlot.text = inventory.BigMonsterSlot.gameObject.GetComponent<BigMonster>().GiveName();
        }
        else
        {
            BigMonsterSlot.text = "Empty";
        }

        if (inventory.SmallMonsterSlotFull)
        {
            SmallMonsterSlot.text = inventory.SmallMonsterSlot.gameObject.GetComponent<SmallMonster>().GiveName();
        }
        else
        {
            SmallMonsterSlot.text = "Empty";
        }

        PC.Player.Inventory.performed += Inventory;
    }
    private void OnDisable()
    {
        PC.Player.Inventory.performed -= Inventory;
    }

    private void OnGUI()
    {
        for (int i = 0; i < FossilSlots.Length; i++)
        {
            if (inventory.SlotIsFull[i])
            {
                FossilSlots[i].text = inventory.Slots[i].gameObject.GetComponent<Fossil>().GiveName();
            }
            else
            {
                FossilSlots[i].text = "Empty";
            }
        }

        if(inventory.BigMonsterSlotFull)
        {
            BigMonsterSlot.text = inventory.BigMonsterSlot.gameObject.GetComponent<BigMonster>().GiveName();
        }
        else
        {
            BigMonsterSlot.text = "Empty";
        }

        if (inventory.SmallMonsterSlotFull)
        {
            SmallMonsterSlot.text = inventory.SmallMonsterSlot.gameObject.GetComponent<SmallMonster>().GiveName();
        }
        else
        {
            SmallMonsterSlot.text = "Empty";
        }
    }

    private void Inventory(InputAction.CallbackContext x)
    {
        if(InventoryUI.activeSelf)
        {
            if (PopUpUI.activeSelf)
            {
                PopUpUI.SetActive(false);
            }
            InventoryUI.SetActive(false);
        }
        else
        {
            InventoryUI.SetActive(true);
        }
    }
    public void InventoryPopUp(GameObject ItemToSwapIn)
    {
        InventoryUI.SetActive(true);
        PopUpUI.SetActive(true);
        inventory.SwapSlotItem = ItemToSwapIn;
    }
    
    public void SwapItem(int Slot)
    {
        if (PopUpUI.activeSelf)
        {
            Destroy(inventory.Slots[Slot]);
            inventory.Slots[Slot] = null;
            inventory.Slots[Slot] = Instantiate(inventory.SwapSlotItem);
            if (PopUpUI.activeSelf)
            {
                PopUpUI.SetActive(false);
            }
            InventoryUI.SetActive(false);
            inventory.SwapSlotItem = null;
        }
    }

    public void NoSwap()
    {
        if (PopUpUI.activeSelf)
        {
            PopUpUI.SetActive(false);
        }
        InventoryUI.SetActive(false);
        inventory.SwapSlotItem = null;
    }
}
