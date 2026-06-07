using UnityEngine;
using System.Collections; 

public class HomingOrb : MonoBehaviour
{
    private Transform target;
    private Vector3 lastChestPosition; // Remembers where the chest is during the impact phase

    [Header("Phase Timers")]
    [SerializeField] private float burstDuration = 0.2f;
    [Tooltip("How long the trail remains absorbing and fading into the player after impact.")]
    [SerializeField] private float remainDurationAfterTouch = 2.0f; 
    private float age = 0f;
    private float fadeAge = 0f;
    private bool isChasing = false;

    [Header("Main Object Movement")]
    [SerializeField] private float initialSpeed = 12f; 
    [SerializeField] private float maxSpeed = 35f;
    [SerializeField] private float acceleration = 30f; 
    [SerializeField] private float steerSpeed = 18f; 
    [SerializeField] private float collectDistance = 0.5f;

    [Header("Particle Pull Settings")]
    [SerializeField] private float particlePullForce = 20f;

    private float currentSpeed;
    private Vector3 velocity;
    private bool isTracking = false;
    private bool isReturningToPool = false; 

    private ParticleSystem pSystem;
    private ParticleSystem.Particle[] particles;

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        if (pSystem != null)
        {
            particles = new ParticleSystem.Particle[pSystem.main.maxParticles];
        }
    }

    public void Setup(Transform targetPlayer)
    {
        target = targetPlayer;
        currentSpeed = initialSpeed;
        age = 0f;
        fadeAge = 0f;
        isChasing = false;
        isTracking = true;
        isReturningToPool = false; 
        
        Vector3 randomCircle = Random.insideUnitSphere;
        randomCircle.y = Mathf.Abs(randomCircle.y) + 0.5f; 
        velocity = randomCircle.normalized * currentSpeed;

        if (pSystem != null)
        {
            pSystem.Clear();
            pSystem.Play();
        }
    }

    void Update()
    {
        // Calculate the current chest position if the player is still alive
        if (target != null)
        {
            lastChestPosition = target.position + (Vector3.up * 1.9f);
        }

        // 1. Handle Main Anchor Movement
        if (isTracking && target != null)
        {
            age += Time.deltaTime;

            if (age < burstDuration)
            {
                // PHASE 1: BURST
                velocity = Vector3.MoveTowards(velocity, Vector3.zero, 15f * Time.deltaTime);
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                // PHASE 2: CHASE
                isChasing = true;

                Vector3 desiredDirection = (lastChestPosition - transform.position).normalized;
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
                
                Vector3 desiredVelocity = desiredDirection * currentSpeed;
                velocity = Vector3.Lerp(velocity, desiredVelocity, steerSpeed * Time.deltaTime);
                
                transform.position += velocity * Time.deltaTime;
            }

            // Check if the master anchor reached the player's chest
            if (isChasing && Vector3.Distance(transform.position, lastChestPosition) < collectDistance)
            {
                StartCoroutine(DelayedReturnToPool());
            }
        }

        // 2. Continuous Particle Math (Runs even during the 2-second absorption phase)
        if (pSystem != null)
        {
            int numParticlesAlive = pSystem.GetParticles(particles);

            // Calculate fade factor if we are in the impact phase
            float alphaFade = 1f;
            if (isReturningToPool)
            {
                fadeAge += Time.deltaTime;
                // Linearly drops from 1 down to 0 over the remainDurationAfterTouch
                alphaFade = Mathf.Clamp01(1f - (fadeAge / remainDurationAfterTouch));
            }

            for (int i = 0; i < numParticlesAlive; i++)
            {
                if (isChasing)
                {
                    Vector3 particleWorldPos = (pSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local) 
                        ? transform.TransformPoint(particles[i].position) 
                        : particles[i].position;

                    // Keep forcefully pulling remaining trail particles into Kratos' chest position
                    Vector3 toTarget = (lastChestPosition - particleWorldPos).normalized;
                    particles[i].velocity = Vector3.Lerp(particles[i].velocity, toTarget * currentSpeed, particlePullForce * Time.deltaTime);
                    
                    // Keep particles artificially alive while chasing
                    if (!isReturningToPool)
                    {
                        particles[i].remainingLifetime = particles[i].startLifetime;
                    }
                    else
                    {
                        // --- SMOOTH FADE OUT ---
                        // Modify the alpha transparency of each individual particle dynamically
                        Color32 originalColor = particles[i].startColor;
                        byte targetAlpha = (byte)(originalColor.a * alphaFade);
                        particles[i].startColor = new Color32(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
                    }
                }
            }

            pSystem.SetParticles(particles, numParticlesAlive);
        }
    }

    private IEnumerator DelayedReturnToPool()
    {
        isReturningToPool = true;
        isTracking = false; // Disables anchor movement updates, but leaves particle updates active!

        if (pSystem != null)
        {
            pSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // Snap the main anchor directly to the chest so remaining local trail lines merge beautifully
        transform.position = lastChestPosition;

        // Wait for 2 seconds while the particles actively suck into the chest and smoothly fade to transparent
        yield return new WaitForSeconds(remainDurationAfterTouch);

        if (pSystem != null)
        {
            pSystem.Clear();
        }

        gameObject.SetActive(false);
    }

}

