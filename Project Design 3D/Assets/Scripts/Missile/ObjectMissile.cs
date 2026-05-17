using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectMissile : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Collider>().enabled = true;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerCollision();
        }
        else if (other.CompareTag("Ground"))
        {
            HandleGroundCollision();
        }
    }

    private void HandlePlayerCollision()
    {
        if (PlayerHealthSystem.Instance != null)
        {
            PlayerHealthSystem.Instance.TakeDamage(); 
        }
        gameObject.SetActive(false); 
    }

    private void HandleGroundCollision()
    {
        gameObject.SetActive(false);
    }

    private void HideMissileTemporarily()
    {
        GetComponent<Collider>().enabled = false;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }
}