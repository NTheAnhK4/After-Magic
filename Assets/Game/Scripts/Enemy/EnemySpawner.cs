using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs;

    private void Start()
    {
        PoolingManager.Spawn(EnemyPrefabs[0], new Vector3(4, 0, 0));
        PoolingManager.Spawn(EnemyPrefabs[0], new Vector3(6, 0, 0));
    }
    
}
