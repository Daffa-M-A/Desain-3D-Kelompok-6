using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [Header("Pengaturan Berat Objek")]
    [Tooltip("1 = Normal. Lebih dari 1 = Berat/Cepat. Kurang dari 1 = Ringan/Melayang.")]
    public float gravityMultiplier = 1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }
}