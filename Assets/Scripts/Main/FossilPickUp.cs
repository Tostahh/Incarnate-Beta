using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FossilPickUp : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;

    private Inventory inventory;
    private bool Done;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            inventory = FindObjectOfType<Inventory>();
            if (inventory.InvetoryIsFull())
            {
                FindObjectOfType<InventoryUIManager>().InventoryPopUp(ItemPrefab);
                Destroy(gameObject);
            }
            else
            {
                for (int i = 0; i < inventory.SlotIsFull.Length; i++)
                {
                    if (inventory.SlotIsFull[i] == false)
                    {
                        inventory.SlotIsFull[i] = true;
                        inventory.Slots[i] = Instantiate(ItemPrefab);
                        inventory.Slots[i].SetActive(false);
                        Done = true;
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
}
