using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnController : NetworkBehaviour
{
    public float spawnRadius = 1;
    public GameObject prefabCaC;
    public GameObject prefabDistance;

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            Destroy(this);
    }

    public GameObject spawn()
    {
        Vector3 position = new Vector3(
        transform.position.x + Random.Range(-spawnRadius, spawnRadius),
        0,
        transform.position.z + Random.Range(-spawnRadius, spawnRadius)
        );

        GameObject prefab = ((Random.Range(0, 2)) == 0 ? prefabCaC : prefabDistance);
        GameObject entity = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
        NetworkServer.Spawn(entity);
        //entity.transform.parent = transform;
        entity.transform.position = position;
        
        return entity;
    }
}
