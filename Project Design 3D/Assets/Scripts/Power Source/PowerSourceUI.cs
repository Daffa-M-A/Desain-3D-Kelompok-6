using UnityEngine;
using UnityEngine.UI; 

public class PowerSourceUI : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Masukkan objek yang memiliki script PowerStorage")]
    [SerializeField] private PowerStorage powerStorage;

    [Tooltip("Masukkan UI Image (yang sudah diset Filled 360) dari Canvas")]
    [SerializeField] private Image powerImage; 


    void Update()
    {
        if (powerStorage != null && powerImage != null)
        {
            powerImage.fillAmount = powerStorage.GetPowerPercentage();
        }
    }
}