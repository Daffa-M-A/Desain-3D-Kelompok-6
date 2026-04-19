using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public GameObject batteryPrefab;
    public float spawnRangeX = 8f;
    public float spawnPosY = 12f;

    // Sekarang mengembalikan nilai 'true' jika baterai berhasil di-spawn
    public bool EvaluateAndSpawn(int currentSecond, bool isFirstCycle)
    {
        if (batteryPrefab == null) return false;

        // 100% di detik ke-8
        if (currentSecond >= 8)
        {
            Spawn();
            return true;
        }

        // Garansi di detik ke-2 siklus pertama
        if (isFirstCycle && currentSecond == 2)
        {
            Spawn();
            return true;
        }

        // Peluang bertingkat
        if (currentSecond >= 2 && currentSecond <= 7)
        {
            float currentChance = GetSpawnChance(currentSecond);
            if (Random.Range(0f, 100f) <= currentChance)
            {
                Spawn();
                return true; // Beritahu manager untuk reset hitungan
            }
        }

        return false; // Tidak ada spawn di detik ini
    }

    private float GetSpawnChance(int second)
    {
        switch (second)
        {
            case 2: return 40f;
            case 3: return 50f;
            case 4: return 60f;
            case 5: return 70f;
            case 6: return 80f;
            case 7: return 90f;
            default: return 0f;
        }
    }

    private void Spawn()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnPosY, 0f);
        Instantiate(batteryPrefab, spawnPos, Quaternion.identity);
    }
}