using UnityEngine;

public class ObjectLaser : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    public float activeDuration = 0.5f;

    void Start()
    {
        // VFX Laser utama otomatis menyala di sini (Play On Awake)

        // Hancurkan laser setelah durasinya habis
        Destroy(gameObject, activeDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Terkena Laser Asli! Nyawa Berkurang!");
            other.gameObject.SetActive(false);
        }
    }
}