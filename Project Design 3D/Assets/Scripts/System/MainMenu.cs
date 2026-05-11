using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ProBuilder;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();// Keluar dari aplikasi saat health mencapai 0
        EditorApplication.isPlaying = false; // Hanya untuk editor, agar tidak keluar dari aplikasi saat testing
    }
}
