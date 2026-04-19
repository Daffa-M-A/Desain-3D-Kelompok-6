using UnityEngine;
using UnityEngine.InputSystem; // Wajib ditambahkan untuk New Input System

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementNewInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Input References")]
    [Tooltip("Masukkan Input Action untuk bergerak (1D Axis)")]
    [SerializeField] private InputActionReference moveAction;

    [Tooltip("Masukkan Input Action untuk lompat (Button)")]
    [SerializeField] private InputActionReference jumpAction;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float horizontalInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Input Action harus di-enable dan di-disable secara manual
    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();

        // Mendaftarkan event saat tombol lompat ditekan (mirip GetKeyDown)
        jumpAction.action.performed += OnJump;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();

        // Melepas event agar tidak terjadi error memori bocor (memory leak)
        jumpAction.action.performed -= OnJump;
    }

    void Update()
    {
        // Membaca nilai pergerakan (bernilai -1 untuk A, dan 1 untuk D)
        horizontalInput = moveAction.action.ReadValue<float>();

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    // Fungsi ini hanya dipanggil TEPAT saat tombol spasi ditekan
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}