using UnityEngine;
using System.Collections; 

public class ObjectBattery : MonoBehaviour
{
    private bool isOnGround = false;

    private void OnEnable()
    {
        isOnGround = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                bool isCollected = inventory.AddBattery();

                if (isCollected)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        else if (other.CompareTag("Ground") && !isOnGround)
        {
            isOnGround = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                Collider myCollider = GetComponent<Collider>();
                Collider groundCollider = other;

                float groundTopEdge = groundCollider.bounds.max.y;
                float batteryBottomEdge = myCollider.bounds.min.y;
                float overlapDistance = groundTopEdge - batteryBottomEdge;

                if (overlapDistance > 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + overlapDistance, transform.position.z);
                }

                rb.isKinematic = true;
            }

            StartCoroutine(DeactivateAfterDelay(3f));
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}