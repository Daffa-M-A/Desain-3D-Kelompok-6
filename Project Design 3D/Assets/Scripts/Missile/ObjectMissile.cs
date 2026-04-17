using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Kurangi health player
        PlayerHealthSystem.Health--;
        if (PlayerHealthSystem.Health <= 0)
        {
            Application.Quit();// Keluar dari aplikasi saat health mencapai 0
            EditorApplication.isPlaying = false; // Hanya untuk editor, agar tidak keluar dari aplikasi saat testing
        }
        else
        {
            StartCoroutine(TerkenaDMG());
        }

        // Hancurkan objek missile setelah mengenai player
        Destroy(gameObject);
    }

    private void HandleGroundCollision()
    {
        // Missile hancur saat menyentuh tanah agar tidak menumpuk di scene
        Destroy(gameObject);
    }

    // Coroutine untuk memberikan efek kebal sementara pada player setelah terkena damage
    IEnumerator TerkenaDMG()
    {
        Physics.IgnoreLayerCollision(6, 8);
        yield return new WaitForSeconds(3);
        Physics.IgnoreLayerCollision(6, 8, false);
    }
}