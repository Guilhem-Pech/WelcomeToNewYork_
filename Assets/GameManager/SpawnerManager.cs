using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnerManager : NetworkBehaviour
{
    private List<SpawnerController> spawners = new List<SpawnerController>();
    // Start is called before the first frame update

    [ServerCallback]
    // Start is called before the first frame update

    private void Awake()
    {
        CheckSpawner();
    }

    [Server]
    void CheckSpawner()
    {
        spawners.Clear();
        spawners.AddRange(GameObject.FindObjectsOfType<SpawnerController>());
        //print("Nombre de spawner : " + spawners.Count);
    }

    [Server]
    public GameObject SpawnEnnemiOnSpawner(GameObject spawner)
    {
        GameObject spawnedEnnemy = new GameObject();
        try
        {
            spawnedEnnemy = spawner.GetComponent<SpawnerController>().spawn();
        }
        catch (System.ArgumentOutOfRangeException e) // If Spawner not found: recheck
        {
            CheckSpawner();
            spawnedEnnemy = spawner.GetComponent<SpawnerController>().spawn();
        }
        return spawnedEnnemy;
    }
    [Server]
    public GameObject SpawnEnnemiRandom()
    {
        int rand = Random.Range(1, spawners.Count + 1);
        //print("random : " + rand);
        GameObject spawnedEnnemy;
        try
        {
            spawnedEnnemy = spawners[rand - 1].spawn();
        }catch (System.ArgumentOutOfRangeException)
        {
            CheckSpawner();
            spawnedEnnemy = spawners[rand - 1].spawn();
        }
        
        return spawnedEnnemy;
    }
}
