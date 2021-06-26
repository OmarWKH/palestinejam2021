using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FightSpawner : MonoBehaviour
{
    [SerializeField] private int maxNumber = 1;
    [SerializeField] private float coolDown = 5f;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject fightPrefab;

    protected internal bool spawning = false;

    private float lastSpawnTime = 0f;
    private int positionIndex = 0;

    void Start()
    {
        lastSpawnTime = -coolDown;
    }
    
    void Update()
    {
        if (spawning && GetComponentsInChildren<Palestinian>().Length < maxNumber)
        {
            if (Time.time - lastSpawnTime >= coolDown)
            {
                Spawn();
                lastSpawnTime = Time.time;
            }
        }
    }

    void Spawn()
    {
        Instantiate(fightPrefab, spawnPositions[positionIndex].position, Quaternion.identity, transform);
        positionIndex = (positionIndex + 1) % spawnPositions.Length;
    }
}
