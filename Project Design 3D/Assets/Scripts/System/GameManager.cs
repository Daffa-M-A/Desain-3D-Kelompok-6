using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// 1. Import the Input System namespace
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    [Tooltip("Masukkan Panel Game Over dari Canvas ke sini")]
    public GameObject gameOverPanel;
    
    [Tooltip("Masukkan Panel Pause Menu dari Canvas ke sini")]
    public GameObject pauseMenuPanel;

    [Header("Game Over Settings")]
    [Tooltip("Durasi jeda (dalam detik) untuk memberi waktu animasi tumbang/collapse player sebelum UI muncul.")]
    [SerializeField] private float gameOverDelay = 2.0f;

    // 2. CREATE THE INPUT ACTION REFERENCE
    [Header("Input System References")]
    [Tooltip("Drag your Pause Input Action reference from your Input Action Asset here")]
    [SerializeField] private InputActionReference pauseActionRef;

    private bool isGameOverTriggered = false;
    private bool isPaused = false;

    // Property so other scripts can check if the game is already over
    public bool IsGameOverTriggered => isGameOverTriggered;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        // Ensure all UI panels are hidden at the start of the level
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    // 3. SUBSCRIBE TO THE ACTION EVENTS
    private void OnEnable()
    {
        if (pauseActionRef != null)
        {
            pauseActionRef.action.started += OnPauseInputTriggered;
            pauseActionRef.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseActionRef != null)
        {
            pauseActionRef.action.started -= OnPauseInputTriggered;
            pauseActionRef.action.Disable();
        }
    }

    // 4. CALLBACK MANUALLY TRIGGERED BY EVENT
    private void OnPauseInputTriggered(InputAction.CallbackContext context)
    {
        // Do not allow pausing if the game is already over!
        if (isGameOverTriggered) return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    #region PAUSE MENU LOGIC

    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        
        Time.timeScale = 0f; // Freeze game logic and movement
        Debug.Log("GameManager: Game Paused.");
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        
        Time.timeScale = 1f; // Unfreeze game logic
        Debug.Log("GameManager: Game Resumed.");
    }

    #endregion

    #region GAME OVER LOGIC

    // OVERLOAD: Defaults to true (with delay) if no parameter is sent
    public void TriggerGameOver()
    {
        TriggerGameOver(true);
    }

    // Main Game Over trigger (PowerStorage passes false to skip delay)
    public void TriggerGameOver(bool useDelay)
    {
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

        // If pause menu is open when game over happens, force hide it
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        if (useDelay)
        {
            Debug.Log($"GameManager: Game Over terdeteksi. Menunggu {gameOverDelay} detik untuk animasi...");
            StartCoroutine(GameOverSequenceRoutine());
        }
        else
        {
            Debug.Log("GameManager: Game Over Instan terdeteksi (Tanpa Delay).");
            ExecuteInstantGameOver();
        }
    }

    private IEnumerator GameOverSequenceRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        ExecuteInstantGameOver();
    }

    private void ExecuteInstantGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f; // Freeze game entirely
        Debug.Log("GameManager: UI Game Over Aktif dan waktu dihentikan.");
    }

    #endregion

    #region SCENE MANAGEMENT

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("COB_MainMenu");
    }

    #endregion
}