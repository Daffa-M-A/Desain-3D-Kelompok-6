using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [Header("Pengaturan Objek")]
    [Tooltip("Masukkan prefab objek yang mau di-spawn di sini")]
    [SerializeField] private GameObject[] objectPrefabs; // Array agar bisa acak lebih dari 1 jenis objek

    [Header("Pengaturan Spawn")]
    [Tooltip("Waktu tunggu antar spawn (dalam detik)")]
    [SerializeField] private float spawnInterval = 1.5f;

    [Tooltip("Batas kiri dan kanan spawn (Sumbu X)")]
    [SerializeField] private float spawnRangeX = 8f;

    [Tooltip("Ketinggian spawn (Sumbu Y) - Pastikan ini di luar kamera")]
    [SerializeField] private float spawnPosY = 12f;

    private float timer;

    void Update()
    {
        // Menjalankan timer
        timer += Time.deltaTime;

        // Jika timer sudah mencapai batas waktu
        if (timer >= spawnInterval)
        {
            SpawnRandomObject();
            timer = 0f; // Reset timer
        }
    }

    private void SpawnRandomObject()
    {
        // Mencegah error jika lupa memasukkan prefab
        if (objectPrefabs.Length == 0) return;

        // 1. Acak objek mana yang akan keluar (jika kamu punya banyak prefab)
        int randomIndex = Random.Range(0, objectPrefabs.Length);

        // 2. Acak posisi sumbu X
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnPosY, 0f);

        // 3. Memunculkan objek
        Instantiate(objectPrefabs[randomIndex], spawnPos, Quaternion.identity);
    }
}