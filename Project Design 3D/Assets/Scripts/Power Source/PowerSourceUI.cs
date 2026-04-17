using UnityEngine;
using UnityEngine.UI; // Wajib ditambahkan untuk mengakses komponen UI Unity

public class PowerSourceUI : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Masukkan objek yang memiliki script PowerStorage")]
    [SerializeField] private PowerStorage powerStorage;

    [Tooltip("Masukkan UI Slider dari Canvas")]
    [SerializeField] private Slider powerSlider;

    void Start()
    {
        // Setup awal agar Slider berfungsi murni sebagai progress bar
        if (powerSlider != null)
        {
            powerSlider.minValue = 0f;
            powerSlider.maxValue = 1f;

            // Sangat penting: Mencegah pemain menggeser bar menggunakan klik mouse
            powerSlider.interactable = false;
        }
    }

    void Update()
    {
        // Setiap frame, update nilai slider sesuai persentase daya yang tersisa
        if (powerStorage != null && powerSlider != null)
        {
            powerSlider.value = powerStorage.GetPowerPercentage();
        }
    }
}