using UnityEngine;

public class DangerSpawner : MonoBehaviour
{
    [Header("Pengaturan Bahaya")]
    public GameObject missilePrefab;
    public GameObject bulletHellPrefab;
    public int swapLimit = 12;
    public float spawnRangeX = 8f;
    public float spawnPosY = 12f;

    private int dangerCounter = 0;
    private bool isMissileMode = true;

    // Fungsi ini dipanggil oleh SpawnManager setiap kali interval bahaya terpenuhi
    public void SpawnDanger()
    {
        GameObject prefabToSpawn = isMissileMode ? missilePrefab : bulletHellPrefab;

        if (prefabToSpawn != null)
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = new Vector3(randomX, spawnPosY, 0f);
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }

        dangerCounter++;

        // Logika Reset dan Ganti Mode
        if (dangerCounter >= swapLimit)
        {
            isMissileMode = !isMissileMode;
            dangerCounter = 0;
            Debug.Log("--- MODE BERGANTI! ---");
        }
    }
}