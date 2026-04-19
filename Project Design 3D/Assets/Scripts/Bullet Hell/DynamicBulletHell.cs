using UnityEngine;

public class DynamicBulletHell : MonoBehaviour
{
    [Header("Pengaturan Sebaran")]
    public float bulletSpeed = 8f;
    public int numberOfBullets = 5;
    public float spreadAngle = 90f;

    void Start()
    {
        ShootDynamicSpread();
        Destroy(gameObject); // Spawner gaib ini boleh hancur, karena pelurunya aman di Pool
    }

    private void ShootDynamicSpread()
    {
        if (numberOfBullets <= 0) return;

        float angleStep = (numberOfBullets > 1) ? spreadAngle / (numberOfBullets - 1) : 0f;
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector3 direction = rotation * Vector3.down;

            // --- PERBEDAANNYA DI SINI ---
            // Mengambil peluru dari gudang (Object Pool)
            GameObject peluruBaru = BulletPool.Instance.GetBullet();

            // Set ulang posisi peluru ke titik tengah spawner
            peluruBaru.transform.position = transform.position;
            peluruBaru.transform.rotation = Quaternion.identity;

            // Setel arah dan kecepatan
            ObjectBullet scriptPeluru = peluruBaru.GetComponent<ObjectBullet>();
            if (scriptPeluru != null)
            {
                scriptPeluru.moveDirection = direction.normalized;
                scriptPeluru.speed = bulletSpeed;
            }
        }
    }
}