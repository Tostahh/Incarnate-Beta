using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;

public class MarketStation : Interactable
{
    public static Action<PlayerStats, Inventory> Upgrade = delegate { };

    [Header("UI")]
    [SerializeField] private GameObject MarketUI;
    [SerializeField] private GameObject BuyUI;
    [SerializeField] private GameObject SellUI;
    [SerializeField] private GameObject[] BuySlots;
    [SerializeField] private Button[] BuyButtons;
    [SerializeField] private TextMeshProUGUI[] BuyPrices;
    [SerializeField] private Button[] SellButtons;
    [SerializeField] private TextMeshProUGUI[] FossilsToSell;
    [SerializeField] private TextMeshProUGUI[] SellPrices;

    private Inventory inventory;
    private PlayerStats PS;
    public override void Awake()
    {
        base.Awake();
        inventory = FindObjectOfType<Inventory>();
        PS = FindObjectOfType<PlayerStats>();
    }
    public void OnGUI()
    {
        if (MarketUI.active)
        {
            ManageSlots();
            ManageButtons();
            ManageText();
        }
    }

    public void ManageSlots()
    {
        if(PS.Axe)
        {
            BuySlots[0].SetActive(true);
        }

        if(PS.Sling)
        {
            BuySlots[1].SetActive(true);
        }
    }
    public void ManageButtons()
    {
        switch (PS.SpearDmg)
        {
            default:
                BuyButtons[0].enabled = false;
                break;
            case 1:
                BuyButtons[0].enabled = inventory.Starbits >= 10;
                break;
            case 2:
                BuyButtons[0].enabled = inventory.Starbits >= 25 && inventory.PearlStone >= 1;
                break;
            case 3:
                BuyButtons[0].enabled = inventory.Starbits >= 50 && inventory.RootStem >= 1;
                break;
        }

        switch (PS.AxeDmg)
        {
            default:
                BuyButtons[1].enabled = false;
                break;
            case 1:
                BuyButtons[1].enabled = inventory.Starbits >= 30;
                break;
            case 2:
                BuyButtons[1].enabled = inventory.Starbits >= 100 && inventory.RootStem >= 1;
                break;
            case 3:
                BuyButtons[1].enabled = inventory.Starbits >= 150 && inventory.DeepfrostOre >= 1;
                break;
        }

        switch (PS.SlingDmg)
        {
            default:
                BuyButtons[2].enabled = false;
                break;
            case 1:
                BuyButtons[2].enabled = inventory.Starbits >= 100;
                break;
            case 2:
                BuyButtons[2].enabled = inventory.Starbits >= 200 && inventory.RootStem >= 5;
                break;
            case 3:
                BuyButtons[2].enabled = inventory.Starbits >= 300 && inventory.DeepfrostOre >= 3;
                break;
        }

        switch (inventory.FossilSlots)
        {
            default:
                BuyButtons[3].enabled = false;
                break;
            case 4:
                BuyButtons[3].enabled = inventory.PearlStone >= 1;
                break;
            case 8:
                BuyButtons[3].enabled = inventory.RootStem >= 3;
                break;
            case 12:
                BuyButtons[3].enabled = inventory.DeepfrostOre >= 5;
                break;
        }

        switch (PS.SearchRange)
        {
            default:
                BuyButtons[4].enabled = false;
                break;
            case 15:
                BuyButtons[4].enabled = inventory.Starbits >= 25;
                break;
            case 20:
                BuyButtons[4].enabled = inventory.Starbits >= 50 && inventory.PearlStone >= 1;
                break;
            case 25:
                BuyButtons[4].enabled = inventory.Starbits >= 100 && inventory.RootStem >= 1;
                break;
        }

        if (!PS.Lantern)
        {
            BuyButtons[5].enabled = inventory.DeepfrostOre >= 1;
        }
        else
        {
            BuyButtons[5].enabled = false;
        }

        for (int i = 0; i < FossilsToSell.Length; i++)
        {
            if (inventory.SlotIsFull[i])
            {
                SellButtons[i].enabled = true;
            }
            else
            {
                SellButtons[i].enabled = false;
            }
        }
    }
    public void ManageText()
    {
        switch (PS.SpearDmg)
        {
            case 1:
                BuyPrices[0].text = "10 Star bits";
                break;
            case 2:
                BuyPrices[0].text = "25 Star bits & 1 Pearl Stone";
                break;
            case 3:
                BuyPrices[0].text = "50 star bits & 1 Root Stem";
                break;
        }

        switch (PS.AxeDmg)
        {
            case 1:
                BuyPrices[1].text = "30 Star bits";
                break;
            case 2:
                BuyPrices[1].text = "100 Star bits & 1 Root Stem";
                break;
            case 3:
                BuyPrices[1].text = "150 star bits & 1 Deepfrost Ore";
                break;
        }

        switch (PS.SlingDmg)
        {
            case 1:
                BuyPrices[2].text = "100 Star bits";
                break;
            case 2:
                BuyPrices[2].text = "200 Star bits & 5 Root Stem";
                break;
            case 3:
                BuyPrices[2].text = "300 star bits & 3 Deepfrost Ore";
                break;
        }

        switch (inventory.FossilSlots)
        {
            case 4:
                BuyPrices[3].text = "1 Pearl Stone";
                break;
            case 8:
                BuyPrices[3].text = "3 Root Stem";
                break;
            case 12:
                BuyPrices[3].text = "5 Deepfrost Ore";
                break;
        }

        switch (PS.SearchRange)
        {
            case 15:
                BuyPrices[4].text = "25 Star bits";
                break;
            case 20:
                BuyPrices[4].text = "50 Star bits & 1 Pearl Stone";
                break;
            case 25:
                BuyPrices[4].text = "100 star bits & 1 Root Stem";
                break;
        }

        switch (PS.Lantern)
        {
            case false:
                BuyPrices[5].text = "1 Deepfrost Ore";
                break;
        }

        for (int i = 0; i < FossilsToSell.Length; i++)
        {
            if (inventory.SlotIsFull[i])
            {
                FossilsToSell[i].text = inventory.Slots[i].gameObject.GetComponent<Fossil>().GiveName();
                SellPrices[i].text = "20 Star Bits";
            }
            else
            {
                FossilsToSell[i].text = "No Fossil";
                SellPrices[i].text = "0 Star Bits";
            }
        }
    }
    public override void Interact(InputAction.CallbackContext Shift)
    {
        if (PlayerInRange)
        {
            if (MarketUI.activeSelf == true)
            {
                MarketUI.SetActive(false);
            }
            else
            {
                MarketUI.SetActive(true);
                if (BuyUI.activeSelf == false)
                {
                    BuyUI.SetActive(true);
                }
                if(SellUI.activeSelf == true)
                {
                    SellUI.SetActive(false);
                }
            }
        }
    }
    public void BuyMenu()
    {
        BuyUI.SetActive(true);
        SellUI.SetActive(false);
    }
    public void SellMenu()
    {
        SellUI.SetActive(true);
        BuyUI.SetActive(false);
    }

    public void PurchaseUpgrade(int purchaseSlot)
    {
        switch (purchaseSlot)
        {
            case 1: // Spear Purchase
                if (PS.SpearDmg == 1 && inventory.Starbits >= 10)
                {
                    inventory.Starbits -= 10;
                    PS.SpearDmg++;
                }
                else if (PS.SpearDmg == 2 && inventory.Starbits >= 25 && inventory.PearlStone >= 1)
                {
                    inventory.Starbits -= 25;
                    inventory.PearlStone--;
                    PS.SpearDmg++;
                }
                else if (PS.SpearDmg == 3 && inventory.Starbits >= 50 && inventory.RootStem >= 1)
                {
                    inventory.Starbits -= 50;
                    inventory.RootStem--;
                    PS.SpearDmg++;
                }
                break;

            case 2: // Axe Purchase
                if (PS.AxeDmg == 1 && inventory.Starbits >= 30)
                {
                    inventory.Starbits -= 30;
                    PS.AxeDmg++;
                }
                else if (PS.AxeDmg == 2 && inventory.Starbits >= 100 && inventory.RootStem >= 1)
                {
                    inventory.Starbits -= 100;
                    inventory.RootStem--;
                    PS.AxeDmg++;
                }
                else if (PS.AxeDmg == 3 && inventory.Starbits >= 150 && inventory.DeepfrostOre >= 1)
                {
                    inventory.Starbits -= 150;
                    inventory.DeepfrostOre--;
                    PS.AxeDmg++;
                }
                break;

            case 3: // Sling Purchase
                if (PS.SlingDmg == 1 && inventory.Starbits >= 100)
                {
                    inventory.Starbits -= 100;
                    PS.SlingDmg++;
                }
                else if (PS.SlingDmg == 2 && inventory.Starbits >= 200 && inventory.RootStem >= 5)
                {
                    inventory.Starbits -= 200;
                    inventory.RootStem -= 5;
                    PS.SlingDmg++;
                }
                else if (PS.SlingDmg == 3 && inventory.Starbits >= 300 && inventory.DeepfrostOre >= 3)
                {
                    inventory.Starbits -= 300;
                    inventory.DeepfrostOre -= 3;
                    PS.SlingDmg++;
                }
                break;

            case 4: // Fossil Slot Purchase
                if (inventory.FossilSlots == 4 && inventory.PearlStone >= 1)
                {
                    inventory.PearlStone--;
                    inventory.UpgradeInventory();
                }
                else if (inventory.FossilSlots == 8 && inventory.RootStem >= 3)
                {
                    inventory.RootStem -= 3;
                    inventory.UpgradeInventory();
                }
                else if (inventory.FossilSlots == 12 && inventory.DeepfrostOre >= 5)
                {
                    inventory.DeepfrostOre -= 5;
                    inventory.UpgradeInventory();
                }
                break;

            case 5: // Search Range Purchase
                if (PS.SearchRange == 15 && inventory.Starbits >= 25)
                {
                    inventory.Starbits -= 25;
                    PS.SearchRange += 5;
                }
                else if (PS.SearchRange == 20 && inventory.Starbits >= 50 && inventory.PearlStone >= 1)
                {
                    inventory.Starbits -= 50;
                    inventory.PearlStone--;
                    PS.SearchRange += 5;
                }
                else if (PS.SearchRange == 25 && inventory.Starbits >= 100 && inventory.RootStem >= 1)
                {
                    inventory.Starbits -= 100;
                    inventory.RootStem--;
                    PS.SearchRange += 5;
                }
                break;

            case 6: // Lantern Purchase
                if (!PS.Lantern && inventory.DeepfrostOre >= 1)
                {
                    inventory.DeepfrostOre--;
                    PS.Lantern = true;
                }
                break;

            default:
                Debug.LogError("Invalid purchase slot.");
                break;
        }

        Upgrade(PS, inventory);
    }
    public void SellFossil(int SellSlot)
    {
        if(inventory.SlotIsFull[SellSlot] == true)
        {
            Destroy(inventory.Slots[SellSlot]);
            inventory.Slots[SellSlot] = null;
            inventory.SlotIsFull[SellSlot] = false;
            inventory.Starbits += 20;
        }
    }
    public void SellItem()
    {

    }
}
