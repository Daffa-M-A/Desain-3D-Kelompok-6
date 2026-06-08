using UnityEngine;

public class TurboModeManager : MonoBehaviour
{
    [Header("Turbo Speed Settings")]
    [SerializeField] private float initialSpeed = 1.0f;
    [SerializeField] private float maxSpeedCap = 1.5f;
    [SerializeField] private float accelerationRate = 0.01f;

    [Header("State")]
    [SerializeField] private bool isTurboModeActive = true;
    
    // NEW: Track whether the game should be frozen/paused right now
    private bool isPaused = false;

    private float currentSpeedMultiplier;
    private float originalFixedDeltaTime;

    void Start()
    {
        currentSpeedMultiplier = initialSpeed;
        originalFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = currentSpeedMultiplier;
    }

    void Update()
    {
        // NEW: If the game is paused or over, DO NOT run the turbo acceleration logic!
        if (isPaused) return;

        if (isTurboModeActive && currentSpeedMultiplier < maxSpeedCap)
        {
            currentSpeedMultiplier += accelerationRate * Time.unscaledDeltaTime;
            currentSpeedMultiplier = Mathf.Min(currentSpeedMultiplier, maxSpeedCap);

            Time.timeScale = currentSpeedMultiplier;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
        }
    }

    /// <summary>
    /// NEW METHOD: Call this instead of setting Time.timeScale directly 
    /// when you want to pause or trigger the Game Over freeze.
    /// </summary>
    public void PauseGame(bool pause)
    {
        isPaused = pause;

        if (pause)
        {
            // Strictly freeze the engine
            Time.timeScale = 0f;
        }
        else
        {
            // Unpause and restore the exact turbo speed we left off at
            Time.timeScale = currentSpeedMultiplier;
            Time.fixedDeltaTime = originalFixedDeltaTime * currentSpeedMultiplier;
        }
    }

    public void SetTurboMode(bool activate)
    {
        isTurboModeActive = activate;
        if (!activate)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
            currentSpeedMultiplier = 1.0f;
        }
    }
}