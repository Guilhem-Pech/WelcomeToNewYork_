using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnerManager : NetworkBehaviour
{
    private List<GameObject> spawners = new List<GameObject>();
    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        print("Nombre de spawner : " + spawners.Count);
    }

    [Server]
    public GameObject SpawnEnnemiOnSpawner(GameObject spawner)
    {
        GameObject spawnedEnnemy = new GameObject();
        spawnedEnnemy = spawner.GetComponent<SpawnController>().spawn();
        return spawnedEnnemy;
    }
    [Server]
    public GameObject SpawnEnnemiRandom()
    {
        int rand = Random.Range(1, spawners.Count + 1);
        print("random : " + rand);
        GameObject spawnedEnnemy = spawners[rand-1].GetComponent<SpawnController>().spawn();
        return spawnedEnnemy;
    }
}
