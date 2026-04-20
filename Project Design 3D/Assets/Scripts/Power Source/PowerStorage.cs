using UnityEditor;
using UnityEngine;

public class PowerStorage : MonoBehaviour
{
    [Header("Power Values")]
    [SerializeField] private float currentPower = 1200f;
    [SerializeField] private float maxPower = 2000f;

    [Header("Rates")]
    [SerializeField] private float drainRate = 45f;

    private bool isGameOver = false;

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
        if (currentPower > maxPower) currentPower = maxPower;

        Debug.Log($"Daya ditambahkan! Current Power: {(int)currentPower}");
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Application.Quit();// Keluar dari aplikasi saat health mencapai 0
        EditorApplication.isPlaying = false; // Hanya untuk editor, agar tidak keluar dari aplikasi saat testing
        Debug.LogError("GAME OVER! Power Source kehabisan daya.");
    }

    // Untuk dibaca oleh UI
    public float GetPowerPercentage()
    {
        return currentPower / 1200f;
    }
}