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

    [Header("Spread Settings")]
    public int spreadBulletsCount = 3;
    public float spreadAngle = 15f;

    [Header("Spin Settings")]
    public float spinSpeed = 90f;
    public int spinLayers = 1;      
    public int spinDepth = 3;       
    public float spinDepthDelay = 0.1f; 

    private float timer = 0f;
    private bool isFiring = false; 

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
        if (!isFiring)
        {
            timer += Time.deltaTime;
        }

        if (spawnerType == SpawnerType.Spin)
        {
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        }

        if (timer >= firingRate && !isFiring)
        {
            if (spawnerType == SpawnerType.Spread)
            {
                FireSpread();
            }
            else if (spawnerType == SpawnerType.Spin)
            {
                StartCoroutine(FireSpinStaggered());
            }

            timer = 0; // Reset ke 0
        }
    }

    private void FireSpread()
    {
        float startAngle = -spreadAngle * (spreadBulletsCount - 1) / 2f;

        for (int i = 0; i < spreadBulletsCount; i++)
        {
            float currentAngle = startAngle + (spreadAngle * i);
            SpawnSingleBullet(currentAngle);
        }

        CheckWaveEnd();
    }

    private IEnumerator FireSpinStaggered()
    {
        isFiring = true;
        float angleStep = 360f / spinLayers;

        for (int depth = 0; depth < spinDepth; depth++)
        {
            for (int i = 0; i < spinLayers; i++)
            {
                float currentAngle = angleStep * i;
                SpawnSingleBullet(currentAngle);
            }
            yield return new WaitForSeconds(spinDepthDelay);
        }

        isFiring = false;
        CheckWaveEnd();
    }

    private void CheckWaveEnd()
    {
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