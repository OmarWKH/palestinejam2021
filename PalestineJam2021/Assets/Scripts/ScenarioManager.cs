using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    [Header("Events Timers")]
    [SerializeField] private float sailingTime = 0f;
    [SerializeField] private float palestiniansSpawnTime = 0f;
    [SerializeField] private float initialMapHidingTime = 1f;
    [SerializeField] private float zionistAssemblyTime = 10f;
    //[SerializeField] private float trapSpawnTime = 15f;
    [SerializeField] private float zionistHidingTime = 20f;
    //[SerializeField] private float evictPalestiniansTime = 25f;
    [SerializeField] private float occupationStartTime = 30f;
    [SerializeField] private float fightsStartTime = 30f;

    // Event conditions
    protected internal bool shouldAssemble;
    
    // Event flags
    // TODO make/use a better system
    private bool initialMapHidden = false;
    private bool zionistsAssembled = false;
    //private bool trapSpawned = false;
    private bool zionistsHidden = false;
    //private bool palestiniansEvicted = false;
    private bool startedOccupation = false;
    private bool sailed = false;
    private bool spanwedPalestinians = false;
    private bool defending = false;
    private bool startedFights = false;

    [Header("Event objects")]
    [SerializeField] private HideOverTime initialMap;
    [SerializeField] private Spawner palestineSpawner;
    [SerializeField] private GameObject shipsParent;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private ShowOverTime fadeOut;
    [SerializeField] private ShowOverTime resist;

    private GameObject trap;

    void Start()
    {
    }

    void Update()
    {
        if (!initialMapHidden && Time.time >= initialMapHidingTime)
        {
            initialMapHidden = true;
            HideInitialMap();
        }

        if (!sailed && Time.time >= sailingTime)
        {
            sailed = true;
            SetSail();
        }

        if (!spanwedPalestinians && Time.time >= palestiniansSpawnTime)
        {
            spanwedPalestinians = true;
            SpawnPalestinians();
        }

        //if (!defending && Time.time >= defendingTime)
        //{
        //    defending = true;
        //    Defend();
        //}

        // TODO mechanic demo phase
        //if (!shipsLanded && Time.time >= shipLandingTime)
        //{

        //}
        // TODO zionists should directly assemble
        // TODO map should start expanding
        // TODO palestenians should start attacking and be pushed by map
        // TODO trap is no longer needed
        if (shouldAssemble && !zionistsAssembled && Time.time >= zionistAssemblyTime)
        {
            zionistsAssembled = true;
            AssembleZionists();
        }

        if (!zionistsHidden && Time.time >= zionistHidingTime)
        {
            zionistsHidden = true;
            HideZionists();
        }
        //if (!trapSpawned && Time.time >= trapSpawnTime)
        //{
        //    trapSpawned = true;
        //    SpawnTrap();
        //}

        //if (!palestiniansEvicted && Time.time >= evictPalestiniansTime)
        //{
        //    palestiniansEvicted = true;
        //    EvictPalestinians();
        //}

        if (!startedOccupation && Time.time >= occupationStartTime)
        {
            startedOccupation = true;
            StartOccupation();
        }

        if (!startedFights && Time.time >= fightsStartTime)
        {
            startedFights = true;
            StartFights();
        }

        //if (!stoppedFights && Time.time >= fightsStopTime)
        //{
        //    stoppedFights= true;
        //    StopFights();
        //}
    }

    void SpawnPalestinians()
    {
        palestineSpawner.Spawn();
    }

    void SetSail()
    {
        foreach (Ship ship in shipsParent.GetComponentsInChildren<Ship>())
        {
            ship.Sail();
        }
    }

    void HideInitialMap()
    {
        initialMap.hiding = true;
    }

    protected internal void AssembleZionists()
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

    void DestroyTrap()
    {
        Destroy(trap);
    }

    void StartOccupation()
    {
        FindObjectOfType<SoliderMap>().StartExpanding();
    }

    void StartFights()
    {
        FindObjectOfType<FightSpawner>().spawning = true;
    }

    protected internal void StopFights()
    {
        resist.fading = true;
        FindObjectOfType<FightSpawner>().spawning = false;
    }

    protected internal void FadeOut()
    {
        fadeOut.fading = true;
        StartCoroutine(EndIt());
    }

    IEnumerator EndIt()
    {
        yield return new WaitForSeconds(1.2f);
        FindObjectOfType<Scenes>().EndScene();
    }
}
