using UnityEngine;

public class ObjectMissile : MonoBehaviour
{
    [Header("Missile Settings")]
    [Tooltip("Efek visual atau partikel saat meledak (Opsional)")]
    [SerializeField] private GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah objek yang ditabrak adalah Player
        if (other.CompareTag("Player"))
        {
            HandlePlayerCollision();
        }
        // Mengecek apakah objek jatuh mengenai lantai/tanah
        else if (other.CompareTag("Ground"))
        {
            HandleGroundCollision();
        }
    }

    private void HandlePlayerCollision()
    {
        // Pesan peringatan di Console
        Debug.LogWarning("Player terkena missile");

        // Hancurkan objek missile setelah mengenai player
        Destroy(gameObject);
    }

    private void HandleGroundCollision()
    {
        // Missile hancur saat menyentuh tanah agar tidak menumpuk di scene
        Destroy(gameObject);
    }
}