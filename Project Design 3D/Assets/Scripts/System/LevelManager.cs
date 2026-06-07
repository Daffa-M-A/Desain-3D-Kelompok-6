using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Pengaturan Level Game")]
    [Range(1, 2)] public int currentLevel = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            currentLevel = PlayerPrefs.GetInt("LevelTerpilih", 1);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LevelUp()
    {
        if (currentLevel < 2)
        {
            currentLevel++;
        }
    }
}