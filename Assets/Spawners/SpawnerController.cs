using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnerController : NetworkBehaviour
{

    public float spawnRadius = 1;
    public int spawnCapacity = 5;

    public float spawnDelay = 2f;

    public bool isSpawning;
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
                if (spawnList.Count == 0)
                    DeactivateSpawn();

                //// Debug.Log("Spawn d'un ennemi !");
                Spawn();
                lastSpawnTime = 0f;
            }
        }
    }

    private void Spawn()
    {
        Vector3 position;

        gameObject.GetComponent<AnimationsReplicationBridge>().SetBoolParam("IsSpawning", true);
        int toSpawn = (spawnList.Count < spawnCapacity)? spawnList.Count : spawnCapacity;
        for (int i = 0; i < toSpawn; i++)
        {
            position = new Vector3(
                transform.position.x + Random.Range(-spawnRadius, spawnRadius),
                0,
                transform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            spawnList[i].transform.position = position;
            spawnList[i].SetActive(true);
            NetworkServer.Spawn(spawnList[i]);
        }

        spawnList.RemoveRange(0, toSpawn);

        if (spawnList.Count == 0)
            DeactivateSpawn();
    }

    public void AddToSpawnList(List<GameObject> ennemyTypesList)
    {
        //// Debug.Log("     Spawner <" + name + "> : Ajout de " + ennemyTypesList.Count + " ennemis à spawn");
        spawnList.AddRange(ennemyTypesList);
        ActivateSpawn();
    }

    private void ActivateSpawn()
    {
        if (isSpawning)
            return;

        //// Debug.Log("     Spawner <" + name + "> : Activation du spawn");
        isSpawning = true;
        lastSpawnTime = 0;
        gameObject.GetComponent<AnimationsReplicationBridge>().playAnimation("Opening");
    }

    private void DeactivateSpawn()
    {
        if (!isSpawning)
            return;

        //// Debug.Log("     Spawner <" + name + "> : Désactivation du spawn");
        isSpawning = false;
        gameObject.GetComponent<AnimationsReplicationBridge>().SetBoolParam("IsClosing", true);
    }
}
