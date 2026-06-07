using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public static OrbManager Instance;

    [Header("Pool Settings")]
    [SerializeField] private GameObject trackingOrbPrefab;
    [SerializeField] private int poolSize = 20; // Pre-allocates 20 orbs in memory

    private List<GameObject> orbPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        orbPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(trackingOrbPrefab, transform);
            obj.SetActive(false); // Keep them hidden until called
            orbPool.Add(obj);
        }
    }

    public void SpawnTrackingOrb(Vector3 spawnPosition, Transform targetPlayer)
    {
        GameObject orb = GetPooledOrb();

        if (orb != null)
        {
            orb.transform.position = spawnPosition;
            orb.SetActive(true); // Triggers Awake/OnEnable if needed

            HomingOrb homing = orb.GetComponent<HomingOrb>();
            if (homing != null)
            {
                homing.Setup(targetPlayer);
            }
        }
    }

    private GameObject GetPooledOrb()
    {
        // 1. Look for an available inactive orb in our pool list
        for (int i = 0; i < orbPool.Count; i++)
        {
            if (!orbPool[i].activeInHierarchy)
            {
                return orbPool[i];
            }
        }

        // 2. Fallback: If player collects things insanely fast and pool runs out, grow the pool dynamically
        Debug.LogWarning("Orb pool exhausted! Spawning extra orb dynamically.");
        GameObject obj = Instantiate(trackingOrbPrefab, transform);
        obj.SetActive(false);
        orbPool.Add(obj);
        return obj;
    }
}