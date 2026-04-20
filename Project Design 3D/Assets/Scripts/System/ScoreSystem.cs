using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update() 
    {
        
    }
}
