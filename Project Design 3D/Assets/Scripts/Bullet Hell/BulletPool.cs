using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // Singleton agar script lain bisa memanggil gudang ini dengan mudah
    public static BulletPool Instance;

    [Header("Pengaturan Gudang")]
    public GameObject bulletPrefab;
    [Tooltip("Jumlah peluru yang disiapkan di awal game")]
    public int poolSize = 50;

    // Queue adalah sistem antrian (Siapa yang masuk duluan, keluar duluan)
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Memproduksi peluru di awal game (Loading screen)
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false); // Matikan peluru (Taruh di gudang)
            pool.Enqueue(obj);    // Masukkan ke daftar antrian
        }
    }

    // Fungsi untuk meminjam peluru dari gudang
    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue(); // Keluarkan dari antrian
            obj.SetActive(true);             // Nyalakan peluru
            return obj;
        }
        else
        {
            // Jika gudang kosong (peluru di layar terlalu banyak), terpaksa buat darurat
            Debug.LogWarning("Gudang Peluru Kosong! Memproduksi peluru darurat.");
            GameObject newObj = Instantiate(bulletPrefab);
            return newObj;
        }
    }

    // Fungsi untuk mengembalikan peluru ke gudang
    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false); // Matikan peluru
        pool.Enqueue(obj);    // Masukkan kembali ke antrian
    }
}