using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnCD = 5f;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private SpawnController controller;
    
    private float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = 0;
    }

    private void Update()
    {
        if (MenuUI.Paused == false && controller.shouldSpawn == true && lastSpawnTime + spawnCD < Time.time)
        {
            lastSpawnTime = Time.time;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
        
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position + spawnPos, Quaternion.identity);
    }
}
