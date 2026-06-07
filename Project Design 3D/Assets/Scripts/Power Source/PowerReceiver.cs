using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PowerReceiver : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Masukkan objek yang memiliki script PowerStorage ke sini")]
    [SerializeField] private PowerStorage mainStorage;
    
    [Tooltip("Masukkan Particle System yang ingin dinyalakan saat setor baterai")]
    [SerializeField] private ParticleSystem depositEffect; 

    [Header("Pengaturan")]
    [SerializeField] private float powerPerBattery = 90f;
    
    [Header("Bonus Settings")]
    [Tooltip("Bonus percentage scaling factor per battery held (0.01 = 1%).")]
    [SerializeField] private float bonusPerBattery = 0.01f;

    public TextMeshProUGUI scoreText; 
    
    // Float internally for exact percentage calculations
    public float score = 0f; 

    private bool playerInRange = false;
    private InventoryManager playerInventory;

    void Start()
    {
        // Automatically find the player's inventory on start so the multiplier works anywhere!
        playerInventory = FindFirstObjectByType<InventoryManager>();
        
        if (playerInventory == null)
        {
            Debug.LogError("PowerReceiver could not find an InventoryManager in the scene!");
        }
    }

    void Update()
    {
        // Update score UI as clean integer
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(score);
        }

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

    /// <summary>
    /// Calculates the score multiplier dynamically based on current inventory.
    /// Your NearMissZone script will call this automatically!
    /// </summary>
    public float GetScoreMultiplier()
    {
        if (playerInventory != null)
        {
            // It reads directly from your Inventory settings (maxBattery = 8)
            int heldBatteries = Mathf.Clamp(playerInventory.currentBattery, 0, playerInventory.maxBattery);
            
            // Formula: 1.0 + (Batteries * 0.01) -> e.g. 4 batteries = 1.04x multiplier (4% bonus)
            return 1f + (heldBatteries * bonusPerBattery);
        }
        
        return 1f; 
    }

    private void ProcessBatteryTransfer()
    {
        if (playerInventory.currentBattery > 0)
        {
            int amount = playerInventory.currentBattery;
            float totalRecovery = amount * powerPerBattery;

            // Kirim daya ke Storage Utama
            mainStorage.AddPower(totalRecovery);

            // Tambahkan skor berdasarkan jumlah baterai yang disetor (dengan multiplier)
            float calculatedTransferScore = 1000f * GetScoreMultiplier();
            score += calculatedTransferScore;

            Debug.Log($"Deposited {amount} batteries with a {GetScoreMultiplier()}x Multiplier! Earned: {calculatedTransferScore} pts.");

            if (depositEffect != null)
            {
                depositEffect.Play();
            }

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
            Debug.Log("Tekan 'W' atau 'Panah Atas' untuk setor baterai.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}