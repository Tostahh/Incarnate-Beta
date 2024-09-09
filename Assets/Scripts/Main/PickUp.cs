using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;

    private Inventory inventory;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            inventory = FindObjectOfType<Inventory>();
            for (int i = 0; i < inventory.SlotIsFull.Length; i++)
            {
                if (inventory.SlotIsFull[i] == false)
                {
                    inventory.SlotIsFull[i] = true;
                    inventory.Slots[i] = Instantiate(ItemPrefab);
                    inventory.Slots[i].SetActive(false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
