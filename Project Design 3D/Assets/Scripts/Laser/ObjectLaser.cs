using UnityEngine;

public class ObjectLaser : MonoBehaviour
{
    [Tooltip("Berapa lama laser ini aktif sebelum menghilang")]
    public float activeDuration = 0.5f;

    void Start()
    {
        // Fitur Unity: Menghancurkan objek ini otomatis setelah sekian detik
        Destroy(gameObject, activeDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Terkena Laser Asli! Nyawa Berkurang!");
            // Panggil fungsi kurangi nyawa/power di sini
        }
    }
}