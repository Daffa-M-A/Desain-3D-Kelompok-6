using UnityEngine;
using System.Collections;

public class ObjectLaser : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    public float activeDuration = 0.5f;


    void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
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

    private void HideLaser()
    {
        GetComponent<Collider>().enabled = false;
    }
}