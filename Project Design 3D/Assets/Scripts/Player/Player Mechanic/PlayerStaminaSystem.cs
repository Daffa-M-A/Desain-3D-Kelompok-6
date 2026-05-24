using UnityEngine;

public class PlayerStaminaSystem : MonoBehaviour
{
    public static PlayerStaminaSystem Instance;

    [Header("Pengaturan Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 5f;    
    public float staminaRegenRate = 5f;    

    void Awake()
    {
        Instance = this;
        currentStamina = maxStamina;
    }

    void Update()
    {

    }

    public void DrainStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public void RegenerateStamina()
    {
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public bool HasStamina()
    {
        return currentStamina > 0f;
    }
}