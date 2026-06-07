using UnityEditor;
using UnityEngine;

public class PowerStorage : MonoBehaviour
{
    [Header("Power Values")]
    [SerializeField] private float currentPower = 1200f;
    [SerializeField] private float maxPower = 2000f;

    [Header("Rates")]
    [SerializeField] private float drainRate = 30f;

    [Header("Soft Cap Settings")]
    [Tooltip("If power goes above this value when charging, it gets forced back down.")]
    [SerializeField] private float powerThresholdTrigger = 1200f;
    [Tooltip("The exact value the power will snap to if it crosses the threshold.")]
    [SerializeField] private float powerForceValue = 1200f;

    private bool isGameOver = false;

    // Public property so other scripts can read current power safely if needed
    public float CurrentPower
    {
        get { return currentPower; }
        private set { currentPower = value; }
    }

    void Update()
    {
        if (isGameOver) return;

        // Pengurangan daya otomatis
        currentPower -= drainRate * Time.deltaTime;

        if (currentPower <= 0)
        {
            currentPower = 0;
            TriggerGameOver();
        }
    }

    // Fungsi Publik yang bisa dipanggil oleh script lain (seperti Receiver)
    public void AddPower(float amount)
    {
        currentPower += amount;

        // 1. Check for your soft cap threshold (force to 1200 if over 1200)
        if (currentPower > powerThresholdTrigger)
        {
            currentPower = powerForceValue;
            Debug.Log($"[Soft Cap Intercepted] Power exceeded {powerThresholdTrigger}! Forced back to: {powerForceValue}");
        }
        // 2. Fallback check for absolute max power limit (2000)
        else if (currentPower > maxPower)
        {
            currentPower = maxPower;
        }

        Debug.Log($"Daya ditambahkan! Current Power: {(int)currentPower}");
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Application.Quit(); // Keluar dari aplikasi saat health mencapai 0
        EditorApplication.isPlaying = false; // Hanya untuk editor, agar tidak keluar dari aplikasi saat testing
        Debug.LogError("GAME OVER! Power Source kehabisan daya.");
    }

    // Untuk dibaca oleh UI
    public float GetPowerPercentage()
    {
        // Keeps percentage calculation matching your current threshold limit
        return currentPower / powerThresholdTrigger;
    }
}