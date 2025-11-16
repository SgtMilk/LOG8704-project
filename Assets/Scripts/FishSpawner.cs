using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class FishObject
{
    public string fishName { get { return m_fishName; } } // Name of fish to spawn
    public GameObject fishPrefab { get { return m_fishPrefab; } } // Prefab of the fish to spawn
    public int maxFishCount { get { return m_maxFishCount; } } // Maximum number of fish to spawn
    public int spawnRate { get { return m_spawnRate; } } // Number of fish to spawn per minute
    public int spawnAmount { get { return m_spawnAmount; } } // Number of fish to spawn each time
    public bool enableSpawner { get { return m_enableSpawner; } }// Enable or disable the spawner

    [Header("Fish Settings")]
    [SerializeField] private string m_fishName;
    [SerializeField] private GameObject m_fishPrefab;
    [SerializeField][Range(0f, 40f)] private int m_maxFishCount;
    [SerializeField][Range(0f, 20f)] private int m_spawnRate;
    [SerializeField][Range(0f, 20f)] private int m_spawnAmount;

    [Header("Fish Spawner Settings")]
    [SerializeField] private bool m_enableSpawner;

    public FishObject(string fishName, GameObject fishPrefab, int maxFishCount, int spawnRate, int spawnAmount)
    {
        this.m_fishName = fishName;
        this.m_fishPrefab = fishPrefab;
        this.m_maxFishCount = maxFishCount;
        this.m_spawnRate = spawnRate;
        this.m_spawnAmount = spawnAmount;
    }

    public void setValues(int maxFishCount, int spawnRate, int spawnAmount)
    {
        this.m_maxFishCount = maxFishCount;
        this.m_spawnRate = spawnRate;
        this.m_spawnAmount = spawnAmount;
    }
}

public class FishSpawner : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();

    public float spawnTimer { get { return m_SpawnTimer; } }
    public Vector3 spawnArea { get { return m_SpawnArea; } }

    [Header("Global settings")]
    [SerializeField][Range(0f, 600f)] private float m_SpawnTimer;
    [SerializeField] private Vector3 m_SpawnArea = new Vector3(10f, 5f, 10f);
    private Transform lureTarget;
    private bool lureActive = false;

    [Header("Fish Objects settings")]
    public FishObject[] FishObjects = new FishObject[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetWaypoints();
        CreateFishGroup();
        InvokeRepeating("SpawnFish", 0.5f, m_SpawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (lureTarget != null)
        {
            if (!lureActive)
            {
                Waypoints.Add(lureTarget);
            }
            lureActive = true;
        }
        else
        {
            if (lureActive)
            {
                Waypoints.Remove(lureTarget);
            }
            lureActive = false;
        }
    }

    void SpawnFish()
    {
        for (int i = 0; i < FishObjects.Count(); i++)
        {
            if (FishObjects[i].enableSpawner && FishObjects[i].fishPrefab != null)
            {
                GameObject tempFish = GameObject.Find(FishObjects[i].fishName);
                if (tempFish.GetComponentInChildren<Transform>().childCount < FishObjects[i].maxFishCount)
                {
                    for (int j = 0; j < FishObjects[i].spawnAmount; j++)
                    {
                        Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
                        GameObject newFish = Instantiate(FishObjects[i].fishPrefab, RandomPosition(), randomRotation);
                        newFish.transform.parent = tempFish.transform;
                        newFish.AddComponent<FishMove>();
                    }
                }
            }
        }
    }

    public Vector3 RandomPosition()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-m_SpawnArea.x / 2, m_SpawnArea.x / 2),
            Random.Range(-m_SpawnArea.y / 2, m_SpawnArea.y / 2),
            Random.Range(-m_SpawnArea.z / 2, m_SpawnArea.z / 2)
        );
        return transform.position + randomPos;
    }

    public Vector3 RandomWaypoint()
    {
        int randomIndex = Random.Range(0, (Waypoints.Count - 1));
        Vector3 randomWaypoint = Waypoints[randomIndex].transform.position;
        return randomWaypoint;
    }

    void CreateFishGroup()
    {
        for (int i = 0; i < FishObjects.Count(); i++)
        {
            GameObject FishGroupSpawn;
            if (FishObjects[i].fishName != null)
            {
                FishGroupSpawn = new GameObject(FishObjects[i].fishName);
                FishGroupSpawn.transform.parent = this.gameObject.transform;
            }
        }
    }

    void GetWaypoints()
    {
        GameObject tempWaypoints = GameObject.Find("Waypoints");
        if (tempWaypoints != null)
        {
            foreach (Transform waypoint in tempWaypoints.transform)
            {
                Waypoints.Add(waypoint);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, m_SpawnArea);
    }
}
