using UnityEngine;

public class PlayerStaminaSystem : MonoBehaviour
{
    public static PlayerStaminaSystem Instance;

    [Header("Pengaturan Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 5f;
    public float staminaRegenRate = 5f;

    // Tambahan gembok kelelahan
    private bool isExhausted = false;

    void Awake()
    {
        Instance = this;
        currentStamina = maxStamina;
    }

    public void DrainStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            isExhausted = true;
        }
    }

    public void RegenerateStamina()
    {
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