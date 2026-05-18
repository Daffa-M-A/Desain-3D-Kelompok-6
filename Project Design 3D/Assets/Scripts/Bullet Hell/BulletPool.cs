using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [Header("Pengaturan Gudang")]
    public GameObject bulletPrefab;
    [Tooltip("Jumlah peluru yang disiapkan di awal game")]
    public int poolSize = 50;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);

            obj.transform.SetParent(transform);

            obj.SetActive(false); 
            pool.Enqueue(obj);    
        }
    }

    public GameObject GetBulletFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue(); 
            return obj;
        }
        else
        {
            Debug.LogWarning("Gudang Peluru Kosong! Memproduksi peluru darurat.");
            GameObject newObj = Instantiate(bulletPrefab);
            newObj.transform.SetParent(transform); 
            return newObj;
        }
    }

    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false); 
        pool.Enqueue(obj);    
    }
}