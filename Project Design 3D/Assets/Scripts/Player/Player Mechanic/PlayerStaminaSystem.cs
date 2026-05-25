using UnityEngine;

public class PlayerStaminaSystem : MonoBehaviour
{
    public static PlayerStaminaSystem Instance;

    [Header("Pengaturan Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 5f;
    public float staminaRegenRate = 5f;

    [Header("Pengaturan Cooldown / Delay")]
    [SerializeField] private float regenDelay = 1.5f; // <-- How long to wait before starting recovery
    private float regenTimer = 0f;                    // <-- Tracks the delay progress

    // Tambahan gembok kelelahan
    private bool isExhausted = false;

    void Awake()
    {
        Instance = this;
        currentStamina = maxStamina;
    }

    public void DrainStamina()
    {
        // Reset the timer completely whenever stamina is actively being used
        regenTimer = regenDelay;

        currentStamina -= staminaDrainRate * Time.deltaTime;

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            isExhausted = true;
        }
    }

    public void RegenerateStamina()
    {
        // Tick down the delay timer first if it hasn't finished yet
        if (regenTimer > 0f)
        {
            regenTimer -= Time.deltaTime;
            return; // Exit out early; do not recover stamina yet
        }

        currentStamina += staminaRegenRate * Time.deltaTime;

        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }

        if (isExhausted && currentStamina > 20f)
        {
            isExhausted = false;
        }
    }

    public bool HasStamina()
    {
        return currentStamina > 0f && !isExhausted;
    }
}