using UnityEngine;
using UnityEngine.UI; 

public class PlayerStaminaUI : MonoBehaviour
{
    [Header("Referensi UI")]
    public Slider staminaSlider;

    void Start()
    {
        if (staminaSlider != null && PlayerStaminaSystem.Instance != null)
        {
            staminaSlider.minValue = 0f;
            staminaSlider.maxValue = PlayerStaminaSystem.Instance.maxStamina;

            staminaSlider.interactable = false;
        }
    }

    void Update()
    {
        if (PlayerStaminaSystem.Instance != null && staminaSlider != null)
        {
            staminaSlider.value = PlayerStaminaSystem.Instance.currentStamina;
        }
    }
}