using System.Collections.Generic;
using UnityEngine;

public class DangerSpawner : MonoBehaviour
{
    public enum DangerType { Missile, BulletHell, Laser }

    [Header("Pengaturan Level")]
    [Tooltip("Ubah ini menjadi 2 untuk menguji kesulitan Level 2")]
    [Range(1, 2)] public int currentLevel = 1;

    [Header("Referensi Prefab")]
    public GameObject missilePrefab;
    public GameObject bulletHellPrefabLvl1;
    public GameObject bulletHellPrefabLvl2;
    public GameObject laserPrefabVertical;
    public GameObject laserPrefabHorizontal;

    [Header("Pengaturan Siklus")]
    public int swapLimit = 12;
    private int dangerCounter = 0;
    private DangerType currentDangerMode = DangerType.Missile;

    [Header("Pengaturan Area")]
    public float spawnRangeX = 8f;
    public float spawnPosY = 12f;
    public float laserRangeY = 5f;
    public float minLaserY = 0f;
    public float screenCenterX = 0f;
    public float screenCenterY = 0f;

    public void SpawnDanger()
    {
        switch (currentDangerMode)
        {
            case DangerType.Missile:
                SpawnMissile();
                break;
            case DangerType.BulletHell:
                SpawnBulletHell();
                break;
            case DangerType.Laser:
                SpawnLaser();
                break;
        }

        dangerCounter++;

        if (dangerCounter >= swapLimit)
        {
            SwitchToRandomMode();
            dangerCounter = 0;
        }
    }

    private void SpawnMissile()
    {
        if (missilePrefab == null) return;

        int missileCount = (currentLevel >= 2) ? 2 : 1;

        for (int i = 0; i < missileCount; i++)
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = new Vector3(randomX, spawnPosY, 0f);
            Instantiate(missilePrefab, spawnPos, Quaternion.identity);
        }

        Debug.Log($"Spawn {missileCount} Missile (Level {currentLevel})");
    }

    private void SpawnBulletHell()
    {
        GameObject prefabToSpawn = (currentLevel >= 2) ? bulletHellPrefabLvl2 : bulletHellPrefabLvl1;

        if (prefabToSpawn != null)
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = new Vector3(randomX, spawnPosY, 0f);
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }

    private void SpawnLaser()
    {
        // LOGIKA LEVEL
        bool isHorizontal = false;
        if (currentLevel >= 2)
        {
            isHorizontal = Random.value > 0.5f;
        }

        Vector3 spawnPos = Vector3.zero;

        if (!isHorizontal)
        {
            // --- MODE VERTIKAL ---
            if (laserPrefabVertical == null) return; // Cegah error jika prefab belum diisi

            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            spawnPos = new Vector3(randomX, screenCenterY, 0f);

            // Spawn prefab khusus Vertikal dengan rotasi bawaan prefab-nya
            Instantiate(laserPrefabVertical, spawnPos, laserPrefabVertical.transform.rotation);

            Debug.Log($"Level {currentLevel}: Laser VERTIKAL (VFX Khusus) di X: {randomX}");
        }
        else
        {
            // --- MODE HORIZONTAL ---
            if (laserPrefabHorizontal == null) return; // Cegah error jika prefab belum diisi

            float randomY = Random.Range(minLaserY, laserRangeY);
            spawnPos = new Vector3(screenCenterX, randomY, 0f);

            // Spawn prefab khusus Horizontal dengan rotasi bawaan prefab-nya
            Instantiate(laserPrefabHorizontal, spawnPos, laserPrefabHorizontal.transform.rotation);

            Debug.Log($"Level {currentLevel}: Laser HORIZONTAL (VFX Khusus) di Y: {randomY}");
        }
    }

    private void SwitchToRandomMode()
    {
        List<DangerType> availableModes = new List<DangerType>();

        if (currentDangerMode != DangerType.Missile) availableModes.Add(DangerType.Missile);
        if (currentDangerMode != DangerType.BulletHell) availableModes.Add(DangerType.BulletHell);
        if (currentDangerMode != DangerType.Laser) availableModes.Add(DangerType.Laser);

        int randomIndex = Random.Range(0, availableModes.Count);
        currentDangerMode = availableModes[randomIndex];
    }
}