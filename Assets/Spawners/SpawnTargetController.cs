using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log("Collision spawner ! : " + (collidedEntity.tag == "Spawner") + ";" + (collidedEntity.GetInstanceID() != gameObject.GetInstanceID()) + ";" + (!IsSpawnerSuscribed(collidedEntity)) + ";");
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
    [Server]
    public float EvaluateSpawner(GameObject Spawner)
    {
        Debug.Log("wsh0");
        float result = 0;
        CheckManager();
        Debug.Log("wsh1");
        float distToSpawner = (Spawner.transform.position - gameObject.transform.position).magnitude;
        if (distToSpawner >= minSpawnDist) {
            Debug.Log("wsh2");
            RaycastHit hitResult;
            if ((Physics.Raycast(gameObject.transform.position, (Spawner.transform.position - gameObject.transform.position), out hitResult, distToSpawner+1f,LayerMask.GetMask("default"),QueryTriggerInteraction.Ignore)))
            {
                Debug.Log("wsh3");
                result = 0f;
            }
            else
            {
                Debug.Log("wsh3");
                result = (1f/ distToSpawner);
            }
        }
        else
        {
            Debug.Log("wsh2");
            result = -1f;
        }
        Debug.Log("wsh4");
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
