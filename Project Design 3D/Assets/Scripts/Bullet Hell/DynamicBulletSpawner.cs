using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBulletSpawner : MonoBehaviour
{
    public enum PatternMode { GridBox, SpreadWave }

    [Header("Mode Pola Peluru")]
    [Tooltip("Pilih GridBox untuk Lvl 1, SpreadWave untuk Lvl 2")]
    public PatternMode currentPattern = PatternMode.GridBox;

    [Header("Pengaturan Umum")]
    public float bulletSpeed = 8f;

    [Header("Pola GridBox (Level 1)")]
    [Tooltip("Jarak antar peluru secara horizontal")]
    public float gridSpacingX = 1.5f;
    [Tooltip("Jarak antar peluru secara vertikal")]
    public float gridSpacingY = 1.5f;

    [Header("Pola SpreadWave (Level 2)")]
    public float spreadAngle = 90f;
    public float timeBetweenRows = 0.2f;

    private int rows;
    private int columns;

    void Start()
    {
        if (currentPattern == PatternMode.GridBox)
        {
            ShootGridBoxPattern();
        }
        else if (currentPattern == PatternMode.SpreadWave)
        {
            RandomizeSpreadPattern();
            StartCoroutine(ShootSpreadRoutine());
        }
    }

    private void ShootGridBoxPattern()
    {
        Vector2Int[] allowedCombinations = new Vector2Int[]
        {
            new Vector2Int(1, 2), 
            new Vector2Int(2, 1), 
            new Vector2Int(2, 2),
            new Vector2Int(2, 4), 
            new Vector2Int(4, 2), 
            new Vector2Int(2, 5),
            new Vector2Int(5, 2)  
        };

        Vector2Int selectedCombo = allowedCombinations[Random.Range(0, allowedCombinations.Length)];
        rows = selectedCombo.x;
        columns = selectedCombo.y;

        Debug.Log($"Level 1 Spawn: Pola Kotak {rows} Baris x {columns} Kolom");

        float startX = transform.position.x - ((columns - 1) * gridSpacingX / 2f);
        float startY = transform.position.y + ((rows - 1) * gridSpacingY / 2f);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 spawnPos = new Vector3(
                    startX + (c * gridSpacingX),
                    startY - (r * gridSpacingY),
                    transform.position.z
                );

                SpawnSingleBullet(spawnPos, Vector3.down); 
            }
        }

        Destroy(gameObject);
    }

    private void RandomizeSpreadPattern()
    {
        int totalBullets = Random.Range(3, 11);
        rows = 1;
        columns = totalBullets;

        if (totalBullets % 2 == 0)
        {
            rows = 2;
            columns = totalBullets / 2;
            if (Random.value > 0.5f)
            {
                int temp = rows;
                rows = columns;
                columns = temp;
            }
        }
        Debug.Log($"Level 2 Spawn: Gelombang Menyebar | {rows} Baris x {columns} Kolom");
    }

    private IEnumerator ShootSpreadRoutine()
    {
        for (int r = 0; r < rows; r++)
        {
            float angleStep = (columns > 1) ? spreadAngle / (columns - 1) : 0f;
            float startAngle = -spreadAngle / 2f;

            for (int c = 0; c < columns; c++)
            {
                float currentAngle = startAngle + (angleStep * c);
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                Vector3 direction = rotation * Vector3.down;

                SpawnSingleBullet(transform.position, direction.normalized);
            }
            yield return new WaitForSeconds(timeBetweenRows);
        }
        Destroy(gameObject);
    }

    private void SpawnSingleBullet(Vector3 position, Vector3 direction)
    {
        if (BulletPool.Instance != null)
        {
            GameObject peluruBaru = BulletPool.Instance.GetBullet();
            peluruBaru.transform.position = position;
            peluruBaru.transform.rotation = Quaternion.identity;
            peluruBaru.transform.localScale = Vector3.one;

            ObjectBullet scriptPeluru = peluruBaru.GetComponent<ObjectBullet>();
            if (scriptPeluru != null)
            {
                scriptPeluru.moveDirection = direction;
                scriptPeluru.speed = bulletSpeed;
            }
        }
    }
}