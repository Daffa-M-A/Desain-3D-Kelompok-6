using UnityEngine;
using System.Collections.Generic;

public class NearMissZone : MonoBehaviour
{
    [Header("Box Dimensions")]
    [Tooltip("The size of your near-miss box detection zone.")]
    [SerializeField] private Vector3 boxSize = new Vector3(3f, 2f, 3f);
    
    [Header("Target Settings")]
    [Tooltip("Select the layer your obstacles are on.")]
    [SerializeField] private LayerMask obstacleLayer;
    
    [Header("Visual Effects")]
    [Tooltip("The particle system that plays when a near miss happens.")]
    [SerializeField] private ParticleSystem nearMissParticles;

    [Header("Score References")]
    [Tooltip("Drag the GameObject that has the PowerReceiver script here.")]
    [SerializeField] private PowerReceiver powerReceiver;
    
    [Tooltip("Score given per second when any obstacle is inside the zone.")]
    [SerializeField] private float scorePerSecond = 65f;
    
    [Tooltip("One-time instant score bonus granted the moment a Rocket enters the zone.")]
    [SerializeField] private int rocketInstantBonus = 500;

    // Tracks obstacles currently inside the box this frame
    private HashSet<int> currentFrameObstacles = new HashSet<int>();
    // Tracks obstacles we already rewarded/processed so we don't double-trigger them
    private HashSet<int> burstTriggeredObstacles = new HashSet<int>();

    private Collider[] hitColliders = new Collider[10];
    private float accumulatedScore = 0f;

    private void Update()
    {
        DetectNearMisses();
    }

    private void DetectNearMisses()
    {
        currentFrameObstacles.Clear();

        // Check for any colliders inside our custom box dimensions
        int count = Physics.OverlapBoxNonAlloc(transform.position, boxSize / 2f, hitColliders, transform.rotation, obstacleLayer);

        for (int i = 0; i < count; i++)
        {
            Collider obstacleCollider = hitColliders[i];
            int obstacleID = obstacleCollider.GetInstanceID();
            currentFrameObstacles.Add(obstacleID);

            // One-time action when ANY obstacle FIRST enters the zone
            if (!burstTriggeredObstacles.Contains(obstacleID))
            {
                TriggerInitialNearMissBurst(obstacleCollider);
                burstTriggeredObstacles.Add(obstacleID);
            }
        }

        // If at least one hazard is in the zone, run the continuous per-second score ticking
        if (count > 0)
        {
            HandleContinuousScore();
        }

        // Clean up obstacles that have left the box zone
        burstTriggeredObstacles.RemoveWhere(id => !currentFrameObstacles.Contains(id));
    }

    private void TriggerInitialNearMissBurst(Collider obstacleCollider)
    {
        Debug.Log($"[Near Miss Entry] Dodged: {obstacleCollider.name} ({obstacleCollider.tag})");

        // 1. Play the particle splash effect
        if (nearMissParticles != null)
        {
            nearMissParticles.Play();
        }

        // 2. ONE-TIME TRIGGER: If it's a rocket, give the instant score drop immediately
        if (obstacleCollider.CompareTag("Rocket") && powerReceiver != null)
        {
            powerReceiver.score += rocketInstantBonus;
            Debug.Log($"[Rocket Bonus] +{rocketInstantBonus} Instant Points!");
        }
    }

    private void HandleContinuousScore()
    {
        if (powerReceiver == null) return;

        // Base continuous calculation (65 points per second)
        accumulatedScore += scorePerSecond * Time.deltaTime;

        // Add whole numbers to PowerReceiver
        if (accumulatedScore >= 1f)
        {
            int pointsToAdd = Mathf.FloorToInt(accumulatedScore);
            powerReceiver.score += pointsToAdd;
            accumulatedScore -= pointsToAdd;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}