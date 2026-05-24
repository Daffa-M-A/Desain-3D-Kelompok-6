using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealingUI : MonoBehaviour
{
    [Header("Referensi UI")]
    public Slider energySlider;

    [Header("Referensi Skill")]
    public PlayerHealingSkill healingSkill;

    [Header("Pengaturan Energi")]
    public float maxEnergy = 100f;

    void Start()
    {
        if (energySlider != null)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = maxEnergy;

            energySlider.interactable = false;
        }
    }

    void Update()
    {
        if (healingSkill != null && energySlider != null)
        {
            energySlider.value = healingSkill.currentEnergy;
        }
    }
}