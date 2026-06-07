using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealingSkill : MonoBehaviour
{
    public static PlayerHealingSkill Instance { get; private set; }

    [Header("Pengaturan Skill")]
    public int healAmount = 1;
    [Tooltip("Jumlah energi yang dibutuhkan untuk satu kali heal")]
    public float energyCost = 30f;
    [Tooltip("Kapasitas maksimum penampung energi skill")]
    public float maxEnergy = 100f;

    [Header("Invincibility Frame Settings")]
    [Tooltip("Durasi kebal (Invincible) setelah memakai skill heal (dalam detik)")]
    [SerializeField] private float invincibilityDuration = 1.0f;
    private float invincibilityTimer = 0f;

    [Header("Animated Burst Settings")]
    [Tooltip("How fast the energy bar animates up. Higher = faster, Lower = slower and smoother.")]
    [SerializeField] private float animationSmoothSpeed = 5f;

    [Header("Debug Energi")]
    public float currentEnergy = 0f; 

    [Header("Input Referensi")]
    public InputActionReference healAction;

    [Header("Referensi Animasi")]
    public Animator animator;

    [Header("Referensi Partikel Efek")]
    [Tooltip("Masukkan Particle System yang ingin dinyalakan saat menggunakan healing skill")]
    [SerializeField] private ParticleSystem healingParticles; 

    private float targetEnergy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        targetEnergy = currentEnergy;
    }

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

    private void Update()
    {
        // 1. Handle I-Frame timer
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        // 2. FIXED SMOOTH ANIMATION ENGINE
        // Lerp smoothly slides currentEnergy toward targetEnergy frame-by-frame
        if (!Mathf.Approximately(currentEnergy, targetEnergy))
        {
            currentEnergy = Mathf.Lerp(currentEnergy, targetEnergy, Time.deltaTime * animationSmoothSpeed);
            
            // Snap to target if it gets extremely close to stop calculating
            if (Mathf.Abs(currentEnergy - targetEnergy) < 0.05f)
            {
                currentEnergy = targetEnergy;
            }
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

        // Instantly drop energy when healing
        targetEnergy = Mathf.Clamp(targetEnergy - energyCost, 0f, maxEnergy);
        currentEnergy = targetEnergy; 
        
        PlayerHealthSystem.Health += healAmount;
        PlayerHealthSystem.Health = Mathf.Clamp(PlayerHealthSystem.Health, 0, maxHealth);

        invincibilityTimer = invincibilityDuration;

        if (healingParticles != null) healingParticles.Play();
        if (animator != null) animator.SetTrigger("IsHealing");

        Debug.Log($"Berhasil Heal! HP Sekarang: {PlayerHealthSystem.Health} | Sisa Energi: {currentEnergy}");
    }

    // CONTINUOUS GAIN: Increases the target smoothly over time
    public void GainEnergy(float amount)
    {
        targetEnergy += amount;
        targetEnergy = Mathf.Clamp(targetEnergy, 0f, maxEnergy);
    }

    // BURST GAIN: Instantly shoots the target value forward, forcing the Lerp in Update to catch up
    public void GainEnergyBurstAnimated(float amount)
    {
        targetEnergy += amount;
        targetEnergy = Mathf.Clamp(targetEnergy, 0f, maxEnergy);
    }

    public bool IsInvincible()
    {
        return invincibilityTimer > 0f;
    }
}