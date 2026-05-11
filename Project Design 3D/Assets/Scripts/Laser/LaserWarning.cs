using UnityEngine;
using System.Collections;

public class LaserWarning : MonoBehaviour
{
    [Header("Referensi")]
    public GameObject objectLaserPrefab;

    [Header("Pengaturan Waktu")]
    public float warningDuration = 1.0f;
    public float blinkInterval = 0.1f;

    [Header("Pengaturan Visual")]
    public bool useMeshBlink = true;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // VFX akan otomatis menyala di sini karena fitur "Play On Awake" bawaan Unity

        StartCoroutine(TelegraphRoutine());
    }

    private IEnumerator TelegraphRoutine()
    {
        float timer = 0f;
        bool isVisible = true;

        while (timer < warningDuration)
        {
            if (useMeshBlink && meshRenderer != null)
            {
                isVisible = !isVisible;
                meshRenderer.enabled = isVisible;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Munculkan Laser Asli
        if (objectLaserPrefab != null)
        {
            Instantiate(objectLaserPrefab, transform.position, transform.rotation);
        }

        // Saat objek ini hancur, anak-anaknya (termasuk VFX Peringatan) akan ikut hancur otomatis
        Destroy(gameObject);
    }
}