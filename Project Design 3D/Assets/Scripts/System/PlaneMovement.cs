using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    [Header("Position Settings")]
    [SerializeField] private Vector3 startPosition = new Vector3(-46f, -12.6f, 22f);
    [SerializeField] private Vector3 endPosition = new Vector3(70f, -12.6f, 22f);

    [Header("Movement Settings")]
    [Tooltip("How fast the plane flies across the screen.")]
    [SerializeField] private float speed = 20f;

    [Header("Random Spawn Settings")]
    [Tooltip("Minimum time (in seconds) before the plane spawns again.")]
    [SerializeField] private float minSpawnTime = 5f;

    [Tooltip("Maximum time (in seconds) before the plane spawns again.")]
    [SerializeField] private float maxSpawnTime = 10f;

    private float timer = 0f;
    private float currentTargetTime;
    private bool isMoving = false;

    void Start()
    {
        // Set the plane to the start position and determine the very first random wait time
        ResetPlaneToWaitState();
    }

    void Update()
    {
        // 1. Handle the randomized timer loop
        timer += Time.deltaTime;
        if (timer >= currentTargetTime)
        {
            StartFlying();
        }

        // 2. Handle the forward movement
        if (isMoving)
        {
            // Move the plane towards the end position
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

            // Once it reaches the destination, hide it/stop it and prepare the next random wait cycle
            if (Vector3.Distance(transform.position, endPosition) < 0.01f)
            {
                ResetPlaneToWaitState();
            }
        }
    }

    private void StartFlying()
    {
        isMoving = true;
    }

    private void ResetPlaneToWaitState()
    {
        // Teleport the plane back to the start so it's ready for its next run
        transform.position = startPosition;
        
        // Stop it from moving while it waits out its timer
        isMoving = false;
        
        // Reset our clock
        timer = 0f;
        
        // Pick a completely new random time between your min and max fields
        currentTargetTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}