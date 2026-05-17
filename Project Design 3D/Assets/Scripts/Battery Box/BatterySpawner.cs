using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [Header("Referensi Pool")]
    [Tooltip("Pastikan tag ini sama dengan yang ada di PoolManager")]
    public string batteryTag = "Battery";

    [Header("Pengaturan Area")]
    public float spawnRangeX = 8f;
    public float spawnPosY = 12f;

    public bool EvaluateAndSpawn(int currentSecond, bool isFirstCycle)
    {
        if (string.IsNullOrEmpty(batteryTag)) return false;

        if (currentSecond >= 8)
        {
            Spawn();
            return true;
        }

        if (isFirstCycle && currentSecond == 2)
        {
            Spawn();
            return true;
        }

        if (currentSecond >= 2 && currentSecond <= 7)
        {
            float currentChance = GetSpawnChance(currentSecond);
            if (Random.Range(0f, 100f) <= currentChance)
            {
                Spawn();
                return true; 
            }
        }

        return false; 
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

        PoolManager.Instance.SpawnFromPool(batteryTag, spawnPos, Quaternion.identity);
    }
}