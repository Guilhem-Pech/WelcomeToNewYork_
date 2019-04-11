using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnTargetController : NetworkBehaviour
{
    public float minSpawnDist;

    private SphereCollider spawnersCollider;
    private GameObject m_parentEntity;
    private SpawnManager manager;
    // Start is called before the first frame update
    void Start()
    {
        m_parentEntity = gameObject.transform.parent.gameObject;
        spawnersCollider = gameObject.GetComponent<SphereCollider>();
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnManager>();
    }

    /* Triggers */
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedEntity;

        if (other.gameObject.transform.parent != null
            && (collidedEntity = other.gameObject.transform.parent.gameObject) != null
            && collidedEntity.tag == "Spawner"
            && collidedEntity.GetInstanceID() != m_parentEntity.GetInstanceID()
            && !manager.IsSpawnerSuscribed(collidedEntity))
        {
            manager.AddSpawner(collidedEntity);
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        GameObject collidedEntity;

        if (other.gameObject.transform.parent != null
            && (collidedEntity = other.gameObject.transform.parent.gameObject) != null
            && collidedEntity.tag == "Spawner"
            && collidedEntity.GetInstanceID() != m_parentEntity.GetInstanceID()
            && manager.IsSpawnerSuscribed(collidedEntity))
        {
            manager.RemoveSpawner(collidedEntity);
        }
    }

    /* Evaluation d'un spawner */
    public float EvaluateSpawner(GameObject Spawner)
    {
        float result = 0;

        float distToSpawner = (Spawner.transform.position - gameObject.transform.position).magnitude;
        if (distToSpawner >= minSpawnDist) {
            RaycastHit hitResult;
            if ((Physics.Raycast(gameObject.transform.position, (Spawner.transform.position - gameObject.transform.position), out hitResult, distToSpawner+1f,LayerMask.GetMask("default"),QueryTriggerInteraction.Ignore)))
            {
                result = 0f;
            }
            else
            {
                result = (1/ distToSpawner);
            }
        }
        else
        {
            result = -1f;
        }

        return result;
    }
}
