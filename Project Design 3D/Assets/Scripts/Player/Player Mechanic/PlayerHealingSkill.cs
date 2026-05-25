using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealingSkill : MonoBehaviour
{
    [Header("Pengaturan Skill")]
    public int healAmount = 1;
    [Tooltip("Jumlah energi yang dibutuhkan untuk satu kali heal")]
    public float energyCost = 30f;

    // --- PLACEHOLDER ENERGI ---
    [Header("Debug Energi")]
    public float currentEnergy = 100f;

    [Header("Input Referensi")]
    public InputActionReference healAction;

    [Header("Referensi Animasi")]
    public Animator animator;

    [Header("Referensi Partikel Efek")]
    [Tooltip("Masukkan Particle System yang ingin dinyalakan saat menggunakan healing skill")]
    [SerializeField] private ParticleSystem healingParticles; // Added particle system reference

    private void OnEnable()
    {
        if (healAction != null)
        {
            healAction.action.Enable();
            healAction.action.performed += OnHealPressed;
        }
    }

    private void OnDisable()
    {
        if (healAction != null)
        {
            healAction.action.Disable();
            healAction.action.performed -= OnHealPressed;
        }
    }

    private void OnHealPressed(InputAction.CallbackContext context)
    {
        ExecuteHeal();
    }

    public void ExecuteHeal()
    {
        int maxHealth = PlayerHealthSystem.Instance.Hearts.Length;

        if (PlayerHealthSystem.Health >= maxHealth)
        {
            Debug.Log("Heal dibatalkan: HP karakter sudah penuh!");
            return; 
        }

        if (currentEnergy < energyCost)
        {
            Debug.Log("Heal dibatalkan: Energi tidak cukup!");
            return; 
        }

        currentEnergy -= energyCost;
        PlayerHealthSystem.Health += healAmount;

        PlayerHealthSystem.Health = Mathf.Clamp(PlayerHealthSystem.Health, 0, maxHealth);

        // Mainkan efek partikel jika referensi sudah dipasang di Inspector
        if (healingParticles != null)
        {
            healingParticles.Play();
        }

        if (animator != null) animator.SetTrigger("IsHealing");

        Debug.Log($"Berhasil Heal! HP Sekarang: {PlayerHealthSystem.Health} | Sisa Energi: {currentEnergy}");
    }
}