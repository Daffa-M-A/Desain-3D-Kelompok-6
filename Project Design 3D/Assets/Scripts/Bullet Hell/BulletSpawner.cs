using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]
    public GameObject bulletPrefab;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    public SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;
    public int maxWaves = 10;
    private int waveCounter = 0;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (spawnerType == SpawnerType.Spin)
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);
        }

        if (timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }

    private void Fire()
    {
        GameObject spawnedBullet = BulletPool.Instance.GetBulletFromPool();

        if (spawnedBullet != null)
        {
            spawnedBullet.transform.position = transform.position;
            spawnedBullet.transform.rotation = transform.rotation;
            spawnedBullet.SetActive(true); 

            ObjectBullet bulletScript = spawnedBullet.GetComponent<ObjectBullet>();
            if (bulletScript != null)
            {
                bulletScript.speed = speed;
                bulletScript.bulletLife = bulletLife;
            }
        }

        waveCounter++;
        if (waveCounter >= maxWaves)
        {
            Destroy(gameObject);
        }
    }
}