using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public BatterySpawner batterySpawner;
    public DangerSpawner dangerSpawner;
    public float dangerIntervalLevel1 = 3.5f;
    public float dangerIntervalLevel2 = 2.0f;

    private float tickTimer = 0f;
    private int currentCycleSecond = 0;
    private bool isFirstCycle = true;
    private float dangerTimer = 0f;

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            tickTimer = 0f;
            currentCycleSecond++;

            if (batterySpawner != null)
            {
                bool hasSpawned = batterySpawner.EvaluateAndSpawn(currentCycleSecond, isFirstCycle);

                if (hasSpawned)
                {
                    currentCycleSecond = 0;
                    tickTimer = 0f;
                    isFirstCycle = false; 
                    Debug.Log("Sistem: Baterai muncul, menghitung ulang dari nol.");
                }
            }

            if (currentCycleSecond >= 8)
            {
                currentCycleSecond = 0;
                isFirstCycle = false;
            }
        }

        dangerTimer += Time.deltaTime;

        float currentDangerInterval = dangerIntervalLevel1;
        if (LevelManager.Instance != null && LevelManager.Instance.currentLevel == 2)
        {
            currentDangerInterval = 2.0f; 
        }

        if (dangerTimer >= currentDangerInterval)
        {
            if (dangerSpawner != null)
            {
                dangerSpawner.SpawnDanger();
            }
            dangerTimer = 0f;
        }
    }
}