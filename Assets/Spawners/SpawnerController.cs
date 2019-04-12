using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnerController : NetworkBehaviour
{

    public float spawnRadius = 1;
    public int spawnCapacity = 5;
    public float spawnDelay = 2f;

    private bool isSpawning;
    private float lastSpawnTime;

    private List<GameObject> spawnList;

    private void Start()
    {
        spawnList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            Destroy(this);

        if (isSpawning)
        {
            lastSpawnTime += Time.deltaTime;

            if(lastSpawnTime >= spawnDelay)
            {
                Spawn();
                lastSpawnTime = 0f;

                if (spawnList.Count == 0)
                    DeactivateSpawn();
            }
        }
    }

    private void Spawn()
    {
        Vector3 position;

        int toSpawn = (spawnList.Count < spawnCapacity)? spawnList.Count : spawnCapacity;
        for (int i = 0; i < toSpawn; i++)
        {
            position = new Vector3(
                transform.position.x + Random.Range(-spawnRadius, spawnRadius),
                0,
                transform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            NetworkServer.Spawn(spawnList[i]);
            spawnList[i].transform.position = position;
        }

        spawnList.RemoveRange(0, toSpawn - 1);
    }

    public void AddToSpawnList(List<GameObject> ennemyTypesList)
    {
        spawnList.AddRange(ennemyTypesList);
        ActivateSpawn();
    }

    private void ActivateSpawn()
    {
        if (isSpawning)
            return;

        isSpawning = true;
        lastSpawnTime = spawnDelay;
    }

    private void DeactivateSpawn()
    {
        if (!isSpawning)
            return;

        isSpawning = false;
    }
}
