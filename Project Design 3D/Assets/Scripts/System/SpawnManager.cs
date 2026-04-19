using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public BatterySpawner batterySpawner;
    public DangerSpawner dangerSpawner;
    public float dangerInterval = 3.5f;

    private float tickTimer = 0f;
    private int currentCycleSecond = 0;
    private bool isFirstCycle = true;
    private float dangerTimer = 0f;

    void Update()
    {
        // 1. JAM BATERAI (Siklus Peluang Bertingkat)
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            tickTimer = 0f;
            currentCycleSecond++;

            if (batterySpawner != null)
            {
                // Cek apakah baterai berhasil spawn
                bool hasSpawned = batterySpawner.EvaluateAndSpawn(currentCycleSecond, isFirstCycle);

                // JIKA SPAWN BERHASIL: Reset hitungan kembali ke nol
                if (hasSpawned)
                {
                    currentCycleSecond = 0;
                    tickTimer = 0f;
                    isFirstCycle = false; // Setelah dapat baterai pertama, garansi siklus 1 selesai
                    Debug.Log("Sistem: Baterai muncul, menghitung ulang dari nol.");
                }
            }

            // Keamanan tambahan: Jika mencapai 8 tanpa spawn (pasti spawn di Evaluator), reset tetap dilakukan
            if (currentCycleSecond >= 8)
            {
                currentCycleSecond = 0;
                isFirstCycle = false;
            }
        }

        // 2. JAM BAHAYA (Interval 12-12)
        dangerTimer += Time.deltaTime;
        if (dangerTimer >= dangerInterval)
        {
            if (dangerSpawner != null)
            {
                dangerSpawner.SpawnDanger();
            }
            // Reset timer bahaya ke nol setiap kali objek bahaya muncul
            dangerTimer = 0f;
        }
    }
}