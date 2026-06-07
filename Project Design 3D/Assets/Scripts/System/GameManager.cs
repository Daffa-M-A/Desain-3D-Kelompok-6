using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    [Tooltip("Masukkan Panel Game Over dari Canvas ke sini")]
    public GameObject gameOverPanel;

    [Header("Game Over Delay Settings")]
    [Tooltip("Durasi jeda (dalam detik) untuk memberi waktu animasi tumbang/collapse player sebelum UI muncul.")]
    [SerializeField] private float gameOverDelay = 2.0f;

    private bool isGameOverTriggered = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TriggerGameOver()
    {
        // Cegah double-trigger jika fungsi ini dipanggil berkali-kali dalam satu frame
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

        Debug.Log($"GameManager: Game Over terdeteksi. Menunggu {gameOverDelay} detik untuk animasi...");

        // Mulai proses penundaan menggunakan Coroutine
        StartCoroutine(GameOverSequenceRoutine());
    }

    private IEnumerator GameOverSequenceRoutine()
    {
        // 1. Biarkan waktu tetap berjalan normal (Time.timeScale = 1f) agar animasi collapse bisa terputar
        yield return new WaitForSeconds(gameOverDelay);

        // 2. Setelah jeda selesai, barulah munculkan UI Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 3. Hentikan waktu permainan setelah animasi selesai ditonton
        Time.timeScale = 0f;
        Debug.Log("GameManager: UI Aktif dan waktu dihentikan.");
    }

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
}