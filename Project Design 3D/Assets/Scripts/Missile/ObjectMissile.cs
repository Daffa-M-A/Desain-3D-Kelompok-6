using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectMissile : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Speed of the rotation. Higher values mean faster spinning.")]
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        float newY = transform.localEulerAngles.y - (rotationSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(90f, newY, 0f);
    }

    private void OnEnable()
    {
        GetComponent<Collider>().enabled = true;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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