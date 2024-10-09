using UnityEngine;
using System;
public class CurrencyPickUp : MonoBehaviour
{
    public static Action<int> StarbitsPickedUp = delegate { };

    private Inventory inventory;
    private bool Done;

    [SerializeField] private int Starbits;
    [SerializeField] private int PearlStone;
    [SerializeField] private int RootStem;
    [SerializeField] private int DeepFrostOre;
    [SerializeField] private int DarkDisasterKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inventory = FindObjectOfType<Inventory>();

            if(Starbits > 0)
            {
                StarbitsPickedUp(Starbits);
            }

            inventory.Starbits += Starbits;
            inventory.PearlStone += PearlStone;
            inventory.RootStem += RootStem;
            inventory.DeepfrostOre += DeepFrostOre;
            inventory.DarkDisasterKey += DarkDisasterKey;

            Destroy(gameObject);
        }
    }
}
