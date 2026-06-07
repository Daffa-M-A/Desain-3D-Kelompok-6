using UnityEngine;
using System.Collections;

public class ObjectLaser : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    public float activeDuration = 0.5f;

    private Coroutine deactivateCoroutine;

    void OnEnable()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = true;

        deactivateCoroutine = StartCoroutine(DeactivateRoutine());
    }

    private void OnDisable()
    {
        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
            deactivateCoroutine = null;
        }
    }

    private IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(activeDuration);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.CompareTag("Player"))
        {
            HandlePlayerCollision();
        }
    }

    private void HandlePlayerCollision()
    {
        if (PlayerHealthSystem.Instance != null)
        {
            PlayerHealthSystem.Instance.TakeDamage();
        }
    }
}