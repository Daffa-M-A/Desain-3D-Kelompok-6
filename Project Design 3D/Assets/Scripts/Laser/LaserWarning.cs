using UnityEngine;
using System.Collections;

public class LaserTelegraph : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Masukkan Prefab Laser Asli ke sini")]
    public GameObject realLaserPrefab;

    [Header("Pengaturan Waktu")]
    public float warningDuration = 1.0f; 
    public float blinkInterval = 0.1f;   

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(TelegraphRoutine());
    }

    private IEnumerator TelegraphRoutine()
    {
        float timer = 0f;
        bool isVisible = true;

        while (timer < warningDuration)
        {
            isVisible = !isVisible; 
            if (meshRenderer != null) meshRenderer.enabled = isVisible;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        if (realLaserPrefab != null)
        {
            Instantiate(realLaserPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}