using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class BatteryHoldUI : MonoBehaviour
{
    [Header("Referensi UI")]
    public TextMeshProUGUI batteryText;

    private InventoryManager inventory;

    void Start()
    {
        inventory = Object.FindFirstObjectByType<InventoryManager>();

        if (inventory == null)
        {
            Debug.LogError("BatteryHoldUI: InventoryManager tidak ditemukan di Scene.");
        }
    }

    void Update()
    {
        if (batteryText != null && inventory != null)
        {
            batteryText.text = $"Battery Hold: {inventory.currentBattery}/{inventory.maxBattery}";
        }
    }
}