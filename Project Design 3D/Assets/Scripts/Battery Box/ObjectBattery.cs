using UnityEngine;

public class ObjectBattery : MonoBehaviour
{
    private bool isOnGround = false;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Jika diambil Player
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                bool isCollected = inventory.AddBattery();

                if (isCollected)
                {
                    Destroy(gameObject);
                }
            }
        }

        // 2. Jika menyentuh Tanah
        else if (other.CompareTag("Ground") && !isOnGround)
        {
            isOnGround = true;

            // Hancurkan dalam 3 detik
            Destroy(gameObject, 3f);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // --- PERBAIKAN POSISI YANG AMAN ---

                Collider myCollider = GetComponent<Collider>();
                Collider groundCollider = other;

                // Mencari titik paling atas dari tanah (Lantai)
                float groundTopEdge = groundCollider.bounds.max.y;

                // Mencari titik paling bawah dari Battery Box
                float batteryBottomEdge = myCollider.bounds.min.y;

                // Menghitung seberapa jauh baterai tenggelam
                float overlapDistance = groundTopEdge - batteryBottomEdge;

                // Menarik baterai ke atas tepat sebesar jarak tenggelamnya
                if (overlapDistance > 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + overlapDistance, transform.position.z);
                }

                // Baru kita bekukan agar gravitasi mati
                rb.isKinematic = true;
            }
        }
    }
}