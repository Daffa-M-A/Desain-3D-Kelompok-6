using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public enum SpawnerType { Spread, Spin }

    [Header("Bullet Attributes")]
    public GameObject bulletPrefab;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    public SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;
    public int maxWaves = 10;
    private int waveCounter = 0;
    public float spinSpeed = 90f;

    [Header("Spread Settings")]
    public int spreadBulletsCount = 3;
    public float spreadAngle = 15f;

    private float timer = 0f;

    void Start()
    {
        if (spawnerType == SpawnerType.Spread)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (spawnerType == SpawnerType.Spin)
        {
            transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (spawnerType == SpawnerType.Spin)
        {
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        }

        if (timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }

    private void Fire()
    {
        if (spawnerType == SpawnerType.Spread)
        {
            float startAngle = -spreadAngle * (spreadBulletsCount - 1) / 2f;

            for (int i = 0; i < spreadBulletsCount; i++)
            {
                float currentAngle = startAngle + (spreadAngle * i);
                SpawnSingleBullet(currentAngle);
            }
        }
        else if (spawnerType == SpawnerType.Spin)
        {
            SpawnSingleBullet(0f);
        }

        waveCounter++;
        if (waveCounter >= maxWaves)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnSingleBullet(float angleOffset)
    {
        GameObject spawnedBullet = BulletPool.Instance.GetBulletFromPool();

        if (spawnedBullet != null)
        {
            spawnedBullet.transform.position = transform.position;

            spawnedBullet.transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, angleOffset);

            spawnedBullet.SetActive(true);

            ObjectBullet bulletScript = spawnedBullet.GetComponent<ObjectBullet>();
            if (bulletScript != null)
            {
                bulletScript.speed = speed;
                bulletScript.bulletLife = bulletLife;
            }
        }
    }
}