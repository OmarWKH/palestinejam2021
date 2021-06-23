using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    [Header("Events Timers")]
    [SerializeField] private float zionistAssemblyTime = 10f;
    [SerializeField] private float trapSpawnTime = 15f;
    [SerializeField] private float zionistHidingTime = 20f;
    [SerializeField] private float evictPalestiniansTime = 25f;
    [SerializeField] private float occupationStartTime = 30f;

    private bool zionistsAssembled = false;
    private bool trapSpawned = false;
    private bool zionistsHidden = false;
    private bool palestiniansEvicted = false;
    private bool startedOccupation = false;

    [SerializeField] private Spawner palestineSpawner;
    [SerializeField] private GameObject shipsParent;
    [SerializeField] private GameObject trapPrefab;

    private GameObject trap;

    void Start()
    {
        palestineSpawner.Spawn();
        foreach (Ship ship in shipsParent.GetComponentsInChildren<Ship>())
        {
            ship.Sail();
        }
    }

    void Update()
    {
        if (!zionistsAssembled && Time.time >= zionistAssemblyTime)
        {
            zionistsAssembled = true;
            AssembleZionists();
        }
        else if (!trapSpawned && Time.time >= trapSpawnTime)
        {
            trapSpawned = true;
            SpawnTrap();
        }
        else if (!zionistsHidden && Time.time >= zionistHidingTime)
        {
            zionistsHidden = true;
            HideZionists();
        }
        else if (!palestiniansEvicted && Time.time >= evictPalestiniansTime)
        {
            palestiniansEvicted = true;
            EvictPalestinians();
        }
        else if (!startedOccupation && Time.time >= occupationStartTime)
        {
            startedOccupation = true;
            StartOccupation();
        }
    }

    void AssembleZionists()
    {
        Zionist[] zionists = FindObjectsOfType<Zionist>();
        float radius = 1;
        for (int i = 0; i < zionists.Length; i++)
        {
            zionists[i].GetComponent<SwarmMovement>().randomizeTarget = false;
            float angle = 360f / zionists.Length * i * Mathf.Deg2Rad;
            float x = radius * Mathf.Cos(angle); // TODO if parented, same bug as in Swarm
            float y = radius * Mathf.Sin(angle);
            zionists[i].GetComponent<SwarmMovement>().target = new Vector2(x, y);
        }
    }

    void SpawnTrap()
    {
        trap = Instantiate(trapPrefab, Vector3.zero, Quaternion.identity);
    }

    void HideZionists()
    {
        Zionist[] zionists = FindObjectsOfType<Zionist>();
        for (int i = 0; i < zionists.Length; i++)
        {
            zionists[i].hidden = true;
            zionists[i].GetComponent<SwarmMovement>().randomizeTarget = false;
            zionists[i].GetComponent<SwarmMovement>().target = Vector3.zero;
        }
    }

    void EvictPalestinians()
    {
        Palestinian[] palestinians = FindObjectsOfType<Palestinian>();
        float radius = 50f;
        for (int i = 0; i < palestinians.Length; i++)
        {
            palestinians[i].evicted = true;
            palestinians[i].GetComponent<SwarmMovement>().randomizeTarget = false;
            float angle = 360f / palestinians.Length * i * Mathf.Deg2Rad;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            palestinians[i].GetComponent<SwarmMovement>().target = new Vector2(x, y);
        }
    }

    void StartOccupation()
    {
        Destroy(trap);
        FindObjectOfType<SoliderMap>().StartExpanding();
    }
}
