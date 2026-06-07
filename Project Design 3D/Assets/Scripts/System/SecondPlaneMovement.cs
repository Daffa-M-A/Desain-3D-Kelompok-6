using UnityEngine;

public class SecondPlaneMovement : MonoBehaviour
{
    [Header("Position Settings")]
    [SerializeField] private Vector3 startPosition = new Vector3(114f, 12.6f, 366f);
    [SerializeField] private Vector3 endPosition = new Vector3(-14.5f, -5.4f, -41.9f);

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 startRotation = new Vector3(0f, 120f, 0f);
    [SerializeField] private Vector3 endRotation = new Vector3(0f, 120f, 0f);

    [Header("Movement Settings")]
    [Tooltip("How fast the plane flies across the screen.")]
    [SerializeField] private float speed = 40f; // Adjusted higher since the distance is quite far

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
        // Set the plane to its unique start state and pick the first random delay
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

        // 2. Handle the forward movement and rotation
        if (isMoving)
        {
            // Move the plane towards the end position (handles X, Y, and Z changes)
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

            // Interpolate rotation just in case start and end rotations ever differ in the Inspector
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                Quaternion.Euler(endRotation), 
                speed * Time.deltaTime
            );

            // Once it reaches the destination close to the camera, hide/reset it
            if (Vector3.Distance(transform.position, endPosition) < 0.05f)
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
        // Teleport the plane back to the far-away start position
        transform.position = startPosition;
        
        // Enforce the explicit starting rotation angles
        transform.rotation = Quaternion.Euler(startRotation);
        
        // Pause movement while waiting out the timer
        isMoving = false;
        timer = 0f;
        
        // Pick a completely new random time between 5-10 seconds
        currentTargetTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}