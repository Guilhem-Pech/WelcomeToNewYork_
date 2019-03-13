using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private List<GameObject> spawners = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        print("Nombre de spawner : " + spawners.Count);
    }

    public GameObject spawnEnnemiOnSpawner(GameObject spawner)
    {
        GameObject spawnedEnnemy = new GameObject();
        spawnedEnnemy = spawner.GetComponent<SpawnController>().spawn();
        return spawnedEnnemy;
    }
    public GameObject spawnEnnemiRandom()
    {
        int rand = Random.Range(1, spawners.Count + 1);
        print("random : " + rand);
        GameObject spawnedEnnemy = spawners[rand-1].GetComponent<SpawnController>().spawn();
        return spawnedEnnemy;
    }
}
