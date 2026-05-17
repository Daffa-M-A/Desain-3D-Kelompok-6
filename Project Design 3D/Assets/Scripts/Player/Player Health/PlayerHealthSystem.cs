using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor; 
#endif

public class PlayerHealthSystem : MonoBehaviour
{
    public static PlayerHealthSystem Instance;

    public static int Health = 3;

    [Header("UI Referensi")]
    public Image[] Hearts;

    [Header("Pengaturan Kebal (I-Frame)")]
    public float iFrameDuration = 3f;
    private bool isInvincible = false;

    void Awake()
    {
        Instance = this;
        Health = 3;
    }

    void Update()
    {
        foreach (Image heart in Hearts)
        {
            heart.enabled = false;
        }
        
        int displayHealth = Mathf.Min(Health, Hearts.Length);
        for (int i = 0; i < displayHealth; i++)
        {
            Hearts[i].enabled = true;
        }
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        Health--;
        Debug.LogWarning($"Player terkena damage! Sisa HP: {Health}");

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(TerkenaDMGRoutine());
        }
    }

    private void Die()
    {
        Debug.Log("Game Over!");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator TerkenaDMGRoutine()
    {
        isInvincible = true;

        Physics.IgnoreLayerCollision(6, 8, true);

        yield return new WaitForSeconds(iFrameDuration);

        Physics.IgnoreLayerCollision(6, 8, false);

        isInvincible = false;
    }
}