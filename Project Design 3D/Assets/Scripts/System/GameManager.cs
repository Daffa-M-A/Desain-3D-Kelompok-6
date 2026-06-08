using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Turbo Integration")]
    [Tooltip("Tarik GameObject yang memiliki script TurboModeManager ke sini")]
    [SerializeField] private TurboModeManager turboManager;

    [Header("UI Panels")]
    [Tooltip("Masukkan Panel Game Over dari Canvas ke sini")]
    public GameObject gameOverPanel;
    
    [Tooltip("Masukkan Panel Pause Menu dari Canvas ke sini")]
    public GameObject pauseMenuPanel;

    [Header("Game Over Settings")]
    [Tooltip("Durasi jeda (dalam detik) untuk memberi waktu animasi tumbang/collapse player sebelum UI muncul.")]
    [SerializeField] private float gameOverDelay = 2.0f;

    [Header("Input System References")]
    [Tooltip("Drag your Pause Input Action reference from your Input Action Asset here")]
    [SerializeField] private InputActionReference pauseActionRef;

    private bool isGameOverTriggered = false;
    private bool isPaused = false;

    public bool IsGameOverTriggered => isGameOverTriggered;

    void Awake()
    {
        Instance = this;
        
        // If turboManager isn't assigned via inspector, try to find it automatically
        if (turboManager == null)
        {
            turboManager = FindFirstObjectByType<TurboModeManager>();
        }
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

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

    private void OnPauseInputTriggered(InputAction.CallbackContext context)
    {
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
        
        // FIXED: Route pause through TurboManager so it won't override it
        if (turboManager != null)
        {
            turboManager.PauseGame(true);
        }
        else
        {
            Time.timeScale = 0f; 
        }

        Debug.Log("GameManager: Game Paused.");
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        
        // FIXED: Route resume through TurboManager to return to your current turbo speed
        if (turboManager != null)
        {
            turboManager.PauseGame(false);
        }
        else
        {
            Time.timeScale = 1f; 
        }

        Debug.Log("GameManager: Game Resumed.");
    }

    #endregion

    #region GAME OVER LOGIC

    public void TriggerGameOver()
    {
        TriggerGameOver(true);
    }

    public void TriggerGameOver(bool useDelay)
    {
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

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
        // FIXED: Changed to Realtime so running at 150% speed doesn't cut the animation short!
        yield return new WaitForSecondsRealtime(gameOverDelay);
        ExecuteInstantGameOver();
    }

    private void ExecuteInstantGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // FIXED: Route freeze through TurboManager so turbo mode halts execution completely
        if (turboManager != null)
        {
            turboManager.PauseGame(true);
        }
        else
        {
            Time.timeScale = 0f; 
        }

        Debug.Log("GameManager: UI Game Over Aktif dan waktu dihentikan.");
    }

    #endregion

    #region SCENE MANAGEMENT

    public void RetryGame()
    {
        // Reset the system via turbo manager if it exists
        if (turboManager != null) turboManager.PauseGame(false);
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        if (turboManager != null) turboManager.PauseGame(false);
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("COB_MainMenu");
    }

    #endregion
}