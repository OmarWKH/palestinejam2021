using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private Spawner palestineSpawner;
    [SerializeField] private GameObject shipsParent;

    void Start()
    {
        palestineSpawner.Spawn();
        foreach (Ship ship in shipsParent.GetComponentsInChildren<Ship>())
        {
            ship.Sail();
        }
    }

    void StartOccupation()
    {
        
    }
}
