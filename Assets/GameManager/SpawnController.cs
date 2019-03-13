using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float spawnRadius = 1;
    public GameObject prefab;



    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject spawn()
    {
    Vector3 position = new Vector3(
    transform.position.x + Random.Range(-spawnRadius, spawnRadius),
    1,
    transform.position.z + Random.Range(-spawnRadius, spawnRadius)
    );
    GameObject entity = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
    //entity.transform.parent = transform;
    entity.transform.position = position;
    GameObject spawnedEntitie = entity ;

        return spawnedEntitie;
    }
}
