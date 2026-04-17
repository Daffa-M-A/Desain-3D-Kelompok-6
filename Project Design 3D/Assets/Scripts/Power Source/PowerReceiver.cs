using UnityEngine;
using UnityEngine.InputSystem;

public class PowerReceiver : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Masukkan objek yang memiliki script PowerStorage ke sini")]
    [SerializeField] private PowerStorage mainStorage;

    [Header("Pengaturan")]
    [SerializeField] private float powerPerBattery = 90f;

    private bool playerInRange = false;
    private InventoryManager playerInventory;

    void Update()
    {
        if (Keyboard.current == null) return;

        // Jika player di zona dan menekan tombol setor
        if (playerInRange && playerInventory != null)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                ProcessBatteryTransfer();
            }
        }
    }

    private void ProcessBatteryTransfer()
    {
        if (playerInventory.currentBattery > 0)
        {
            int amount = playerInventory.currentBattery;
            float totalRecovery = amount * powerPerBattery;

            // Kirim daya ke Storage Utama
            mainStorage.AddPower(totalRecovery);

            // Kosongkan tas player
            playerInventory.currentBattery = 0;
        }
        else
        {
            Debug.Log("Inventaris kosong! Cari Battery Box dulu.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<InventoryManager>();
            Debug.Log("Tekan 'W' atau 'Panah Atas' untuk setor baterai.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}