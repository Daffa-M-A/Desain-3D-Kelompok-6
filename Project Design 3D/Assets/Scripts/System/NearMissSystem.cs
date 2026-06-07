using UnityEngine;
using System.Collections.Generic;

public class NearMissZone : MonoBehaviour
{
    [Header("Box Dimensions")]
    [SerializeField] private Vector3 boxSize = new Vector3(3f, 2f, 3f);
    
    [Header("Target Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem nearMissParticles;

    [Header("Score References")]
    [SerializeField] private PowerReceiver powerReceiver;
    [SerializeField] private float scorePerSecond = 65f;
    [SerializeField] private int rocketInstantBonus = 500;

    [Header("Healing Skill Integration (Burst)")]
    [SerializeField] private PlayerHealingSkill playerHealingSkill;
    [SerializeField] private float energyBurstAmount = 15f;
    [SerializeField] private float energyGainCooldown = 1.5f;

    [Header("Healing Skill Integration (Continuous)")]
    [SerializeField] private float energyContinuousPerSecond = 5f;

    private float nextEnergyGainTime = 0f;
    private HashSet<int> currentFrameObstacles = new HashSet<int>();
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
        int count = Physics.OverlapBoxNonAlloc(transform.position, boxSize / 2f, hitColliders, transform.rotation, obstacleLayer);

        for (int i = 0; i < count; i++)
        {
            Collider obstacleCollider = hitColliders[i];
            int obstacleID = obstacleCollider.GetInstanceID();
            currentFrameObstacles.Add(obstacleID);

            if (!burstTriggeredObstacles.Contains(obstacleID))
            {
                TriggerInitialNearMissBurst(obstacleCollider);
                burstTriggeredObstacles.Add(obstacleID);
            }
        }

        if (count > 0)
        {
            HandleContinuousScore();
            HandleContinuousEnergyGain(); 
        }

        burstTriggeredObstacles.RemoveWhere(id => !currentFrameObstacles.Contains(id));
    }

    private void TriggerInitialNearMissBurst(Collider obstacleCollider)
    {
        if (nearMissParticles != null) nearMissParticles.Play();

        if (obstacleCollider.CompareTag("Rocket") && powerReceiver != null)
        {
            powerReceiver.score += rocketInstantBonus;
        }

        if (playerHealingSkill != null)
        {
            if (Time.time >= nextEnergyGainTime)
            {
                // CALLS THE ANIMATED BURST METHOD Instead of instant assignment
                playerHealingSkill.GainEnergyBurstAnimated(energyBurstAmount);
                nextEnergyGainTime = Time.time + energyGainCooldown;
            }
        }
    }

    private void HandleContinuousEnergyGain()
    {
        if (playerHealingSkill != null)
        {
            playerHealingSkill.GainEnergy(energyContinuousPerSecond * Time.deltaTime);
        }
    }

    private void HandleContinuousScore()
    {
        if (powerReceiver == null) return;
        accumulatedScore += scorePerSecond * Time.deltaTime;
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