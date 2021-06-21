using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int number = 1;
    // TODO spawnRadius is same as targetRadius in SwarmMovement.cs
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private GameObject prefab;
    
    protected internal void Spawn()
    {
        for (int i = 0; i < number; i++)
        {
            Vector2 position = Random.insideUnitCircle * spawnRadius;
            GameObject spawned = Instantiate(prefab, position, Quaternion.identity, transform);
            spawned.name = $"{prefab.name}_{i}";
        }
    }
}
