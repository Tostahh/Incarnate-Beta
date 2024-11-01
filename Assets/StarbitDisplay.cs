using UnityEngine;
using TMPro;

public class StarbitDisplay : MonoBehaviour
{
    private TextMeshProUGUI Display;
    private Inventory inventory;
    private void OnEnable()
    {
        Display = GetComponent<TextMeshProUGUI>();
        inventory = FindFirstObjectByType<Inventory>();
    }

    private void OnGUI()
    {
        if(inventory && Display)
        {
            Display.text = "SB: " + inventory.Starbits.ToString();
        }
        else if (Display)
        {
            Display.text = "SB: Error";
        }
    }
}
