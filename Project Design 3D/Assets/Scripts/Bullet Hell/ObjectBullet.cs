using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ObjectBullet : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public float speed;

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah objek yang ditabrak adalah Player
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Terkena Peluru Touhou!");
            HandlePlayerCollision();
            gameObject.SetActive(false); // JANGAN DESTROY. Matikan saja.
        }
        // Mengecek apakah objek jatuh mengenai lantai/tanah
        else if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false); // JANGAN DESTROY. Matikan saja.
        }
    }

    // Fitur Rahasia: Setiap kali objek dimatikan (SetActive(false)),
    // fungsi ini otomatis berjalan dan mengembalikan dirinya ke gudang.
    private void OnDisable()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
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

    // Coroutine untuk memberikan efek kebal sementara pada player setelah terkena damage
    IEnumerator TerkenaDMG()
    {
        Physics.IgnoreLayerCollision(6, 8);
        yield return new WaitForSeconds(3);
        Physics.IgnoreLayerCollision(6, 8, false);
    }
}