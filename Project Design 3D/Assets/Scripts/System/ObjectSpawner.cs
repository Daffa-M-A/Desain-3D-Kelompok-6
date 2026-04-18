using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject batteryPrefab;
    [SerializeField] private GameObject missilePrefab;

    [Header("Spawn Settings")]
    [Range(0, 100)]
    [SerializeField] private float batterySpawnChance = 50f; // Peluang muncul (0-100%)
    [SerializeField] private float missileSpawnInterval = 4f; // Missile tetap pakai interval biasa agar variatif

    [Header("Range Settings")]
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnPosY = 12f;

    private float timerSeconds = 0f;
    private int currentSecond = 0;
    private float missileTimer = 0f;
    private bool firstBatterySpawned = false;

    // Daftar detik di mana baterai MUNGKIN muncul (2, 3, 4, 5, 6, 7, 8)
    private List<int> batterySpawnTicks = new List<int> { 2, 3, 4, 5, 6, 7, 8 };

    void Update()
    {
        // 1. Logika Perhitungan Detik (Tick)
        timerSeconds += Time.deltaTime;

        if (timerSeconds >= 1f)
        {
            timerSeconds -= 1f; // Reset timer tapi simpan sisa mili detiknya
            currentSecond++;

            HandleBatteryTick();
        }

        // 2. Logika Missile (Tetap menggunakan interval agar tidak terlalu kaku)
        missileTimer += Time.deltaTime;
        if (missileTimer >= missileSpawnInterval)
        {
            SpawnObject(missilePrefab);
            missileTimer = 0f;
        }
    }

    private void HandleBatteryTick()
    {
        // ATURAN 1: Selalu muncul di 2 detik pertama permainan
        if (currentSecond == 2 && !firstBatterySpawned)
        {
            SpawnObject(batteryPrefab);
            firstBatterySpawned = true;
            Debug.Log("Spawn Baterai Pertama (Dijamin)");
            return;
        }

        // ATURAN 2: Setelah itu, cek apakah detik sekarang ada di dalam daftar 2-8
        // Kita gunakan modulo 8 agar polanya berulang setiap siklus 8 detik
        int secondInCycle = (currentSecond % 8 == 0) ? 8 : currentSecond % 8;

        if (currentSecond > 2 && batterySpawnTicks.Contains(secondInCycle))
        {
            // Cek probabilitas
            float roll = Random.Range(0f, 100f);
            if (roll <= batterySpawnChance)
            {
                SpawnObject(batteryPrefab);
                Debug.Log($"Spawn Baterai di detik ke-{currentSecond} (Berhasil Roll)");
            }
        }
    }

    private void SpawnObject(GameObject prefab)
    {
        if (prefab == null) return;

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnPosY, 0f);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}