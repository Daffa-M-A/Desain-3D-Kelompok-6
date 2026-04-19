using UnityEngine;

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
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Terkena Peluru Touhou!");
            gameObject.SetActive(false); // JANGAN DESTROY. Matikan saja.
        }
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
}