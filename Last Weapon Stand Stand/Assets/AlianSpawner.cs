using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlianSpawner : MonoBehaviour
{
    [SerializeField] internal float      minSpawnRate = 0;
    [SerializeField] internal float      maxSpawnRate = 2;
    [SerializeField] private  float      width        = 200;
    [SerializeField] private  float      depth        = 50;
    [SerializeField] private  GameObject alianPrefab;

    private IAlienCounter _alienCounter;

    private float timer = 0;
    private void OnDrawGizmos()
    {
        Vector3 position = transform.position+new Vector3(0,1,0);
        position = new Vector3(0,1,0);
        Vector3 size     = new Vector3(width,2,depth);

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(1, 1, 1, .2f);
        Gizmos.DrawCube(position, size);
        
        Gizmos.color = new Color(0, 0, 1);
        Gizmos.DrawWireCube(position, size);
    }

    void Start()
    {
        _alienCounter = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounter>()[0];
    }


    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Spawn();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = Random.Range(minSpawnRate, maxSpawnRate);
    }

    private void Spawn()
    {
        float   x             = Random.Range(-width / 2, width / 2);
        float   z             = Random.Range(-depth / 2, depth / 2);
        Vector3 spawnPosition = transform.TransformPoint(new Vector3(x, 0, z));

        GameObject go = Instantiate(alianPrefab, spawnPosition, quaternion.identity);
        _alienCounter.AdjustCount(1);
        go.name = $"Alien #{_alienCounter.Count}";
    }
}
