using System.Collections;
using UnityEngine;

public class NearMissSystem : MonoBehaviour
{
    private bool playerInRange = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.LogWarning("Player entered near miss range!");
        }
    }
}