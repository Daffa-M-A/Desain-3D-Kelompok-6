using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ObjectBullet : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public float speed;

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Terkena Peluru Touhou!");
            HandlePlayerCollision();
            gameObject.SetActive(false); 
        }
        else if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false); 
        }
    }

    private void OnDisable()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }

    private void HandlePlayerCollision()
    {
        Debug.LogWarning("Player terkena missile");

        PlayerHealthSystem.Health--;
        if (PlayerHealthSystem.Health <= 0)
        {
            Application.Quit();
            EditorApplication.isPlaying = false; 
        }
        else
        {
            StartCoroutine(TerkenaDMG());
        }

        Destroy(gameObject);
    }

    IEnumerator TerkenaDMG()
    {
        Physics.IgnoreLayerCollision(6, 8);
        yield return new WaitForSeconds(3);
        Physics.IgnoreLayerCollision(6, 8, false);
    }
}