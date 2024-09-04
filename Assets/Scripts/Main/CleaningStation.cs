using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CleaningStation : Interactable
{
    [SerializeField] private GameObject CleaningUI;
    [SerializeField] private TextMeshProUGUI[] UISlotsText;

    private Inventory inventory;
    public override void Awake()
    {
        base.Awake();
        inventory = FindObjectOfType<Inventory>();
    }
    public override void Interact(InputAction.CallbackContext Shift)
    {
        if (PlayerInRange)
        {
            if (CleaningUI.activeSelf == true)
            {
                CleaningUI.SetActive(false);
            }
            else
            {
                CleaningUI.SetActive(true);
                DisplayNames();
            }
        }
    }

    public void DisplayNames()
    {
        for (int i = 0; i < UISlotsText.Length; i++)
        {
            if (inventory.SlotIsFull[i])
            {
                UISlotsText[i].text = inventory.Slots[i].gameObject.GetComponent<Fossil>().GiveName();
            }
            else
            {
                UISlotsText[i].text = "Empty";
            }
        }
    }
    public void SelectFossil(int i)
    {
        if (inventory.SlotIsFull[i])
        {
            GameObject Fossil = Instantiate(inventory.Slots[i]);
            inventory.SlotIsFull[i] = false;
            inventory.Slots[i] = null;
            FindObjectOfType<SceneManagment>().ChangeScene("Rhythm");
        }
    }
}
