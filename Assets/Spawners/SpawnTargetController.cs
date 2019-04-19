using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class SpawnTargetController : NetworkBehaviour
{
    public float minSpawnDist;

    private Dictionary<int, GameObject> currentSpawners;

    private SphereCollider spawnersCollider;
    private SpawnManager manager;

    [ServerCallback]
    void Awake()
    {
        currentSpawners = new Dictionary<int, GameObject>();
        spawnersCollider = gameObject.GetComponent<SphereCollider>();
        CheckManager();
    }

    [ServerCallback]
    private void Update()
    {
        CheckManager();
    }

    /* Triggers */
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedEntity = other.gameObject;
        CheckManager();

        if (collidedEntity.tag == "Spawner"
            && collidedEntity.GetInstanceID() != gameObject.GetInstanceID()
            && !IsSpawnerSuscribed(collidedEntity))
        {
            AddSpawner(collidedEntity);
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        GameObject collidedEntity = other.gameObject;
        CheckManager();

        if (collidedEntity.tag == "Spawner"
            && collidedEntity.GetInstanceID() != gameObject.GetInstanceID()
            && IsSpawnerSuscribed(collidedEntity))
        {
            RemoveSpawner(collidedEntity);
        }
    }

    /* Evaluation d'un spawner */
    public float EvaluateSpawner(GameObject Spawner)
    {
        float result = 0;
        CheckManager();
        //Debug.Log("     Spawner <" + Spawner.name + "> :");

        float distToSpawner = (Spawner.transform.position - gameObject.transform.position).magnitude;
        //Debug.Log("         distToSpawner : " + distToSpawner);
        result = (distToSpawner - minSpawnDist);
        //Debug.Log("         Calcul du result : " + result);

        NavMeshHit hitResult;
        if ((NavMesh.Raycast(gameObject.transform.position, Spawner.transform.position, out hitResult, NavMesh.AllAreas)))
        {
            result = 0 -(Mathf.Abs(result)) - (minSpawnDist);
            //Debug.Log("         Collision -> correction du result : " + result);
        }

        return result;
    }

    /* Pop. Managers */
    public Dictionary<int, GameObject> GetCurrentSpawners()
    {
        return currentSpawners;
    }

    [Server]
    private void AddSpawner(GameObject spawnerToAdd)
    {
        currentSpawners.Add(spawnerToAdd.GetInstanceID(), spawnerToAdd);
        OnPopChange();
    }

    [Server]
    private void RemoveSpawner(GameObject spawnerToRemove)
    {
        currentSpawners.Remove(spawnerToRemove.GetInstanceID());
        OnPopChange();
    }

    private bool IsSpawnerSuscribed(GameObject entity)
    {
        return currentSpawners.ContainsKey(entity.GetInstanceID());
    }

    private void OnPopChange()
    {
        if (manager != null)
            manager.OnCurrentSpawnersChange();
    }

    /* Misc */
    public void CheckManager()
    {
        if (manager == null && GameObject.FindGameObjectWithTag("GameController") != null)
            manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().waveMan.spawnMan;
    }
}
