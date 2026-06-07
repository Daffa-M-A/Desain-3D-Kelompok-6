using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpCooldown = 0.5f;
    private float nextJumpTime = 0f;
    private bool requireNewSprintPress = false;

    [Header("Input References")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;
    [SerializeField] private InputActionReference fastFallAction;

    [Header("Gravity Settings")]
    [SerializeField] private float normalFallMultiplier = 2.5f;
    [SerializeField] private float fastFallMultiplier = 10f;

    [Header("Referensi Animasi")]
    public Animator animator;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float horizontalInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();

        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJump;
        }

        if (sprintAction != null) sprintAction.action.Enable();

        if (fastFallAction != null) fastFallAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();

        if (jumpAction != null)
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJump;
        }

        if (sprintAction != null) sprintAction.action.Disable();

        if (fastFallAction != null) fastFallAction.action.Disable();
    }

    void Update()
    {
        horizontalInput = moveAction.action.ReadValue<float>();
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        bool isSprintHeld = false;
        if (sprintAction != null && sprintAction.action != null)
        {
            isSprintHeld = sprintAction.action.IsPressed();
        }

        if (!isSprintHeld)
        {
            requireNewSprintPress = false;
        }

        float currentSpeed = walkSpeed;

        if (PlayerStaminaSystem.Instance != null)
        {
            if (isSprintHeld && !PlayerStaminaSystem.Instance.HasStamina())
            {
                requireNewSprintPress = true;
            }

            if (isSprintHeld && isMoving && PlayerStaminaSystem.Instance.HasStamina() && !requireNewSprintPress)
            {
                currentSpeed = sprintSpeed;
                PlayerStaminaSystem.Instance.DrainStamina();
            }
            else
            {
                currentSpeed = walkSpeed;
                PlayerStaminaSystem.Instance.RegenerateStamina();
            }
        }
        else
        {
            currentSpeed = isSprintHeld ? sprintSpeed : walkSpeed;
        }

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

        if (isMoving)
        {
            Vector3 faceDirection = moveDirection.normalized;

            Quaternion targetRotation = Quaternion.LookRotation(faceDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
        }

// --- ANIMATION CONTROLLER ---
        if (animator != null)
        {
            float speedParameter = 0f;

            if (isMoving)
            {
                // If currentSpeed is greater than walking speed, set to 2 (Sprint), else set to 1 (Walk)
                speedParameter = (currentSpeed > walkSpeed) ? 2f : 1f;
            }

            // Using SmoothDamp or a simple Lerp makes the parameter transition smoothly in the Blend Tree
            float currentAnimSpeed = animator.GetFloat("Speed");
            float smoothedSpeed = Mathf.MoveTowards(currentAnimSpeed, speedParameter, Time.deltaTime * 5f);

            animator.SetFloat("Speed", smoothedSpeed);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0)
        {
            bool isFastFalling = false;
            if (fastFallAction != null && fastFallAction.action != null)
            {
                isFastFalling = fastFallAction.action.IsPressed();
            }

            float currentMultiplier = isFastFalling ? fastFallMultiplier : normalFallMultiplier;

            rb.linearVelocity += Vector3.up * Physics.gravity.y * (currentMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && Time.time >= nextJumpTime)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded)
            {
                nextJumpTime = Time.time + jumpCooldown;
            }

            isGrounded = true;
        }
    }
}