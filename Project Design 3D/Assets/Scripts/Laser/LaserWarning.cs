using UnityEngine;
using System.Collections;

public class LaserWarning : MonoBehaviour
{
    [Header("Referensi Pool")]
    public string realLaserTag;

    [Header("Pengaturan Waktu")]
    public float warningDuration = 1.0f;
    public float blinkInterval = 0.1f;

    [Header("Pengaturan Visual")]
    public bool useMeshBlink = true;

    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnEnable()
    {
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

        PoolManager.Instance.SpawnFromPool(realLaserTag, transform.position, transform.rotation);

        gameObject.SetActive(false);
    }
}