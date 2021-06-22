using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int number = 1;
    [SerializeField] private Position positionMode = Position.RandomInRadius;
    // TODO spawnRadius is same as targetRadius in SwarmMovement.cs
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private GameObject prefab;
    
    protected internal void Spawn()
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 position = Vector3.zero;
            switch (positionMode)
            {
                case Position.RandomInRadius:
                    position = Random.insideUnitCircle * spawnRadius;
                    break;
                case Position.Parent:
                    position = transform.position;
                    break;
                default:
                    break;
            }

            GameObject spawned = Instantiate(prefab, position, Quaternion.identity, transform);
            spawned.name = $"{prefab.name}_{i}";
        }
    }
}

public enum Position
{
    RandomInRadius,
    Parent,
}