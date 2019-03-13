using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetingSystem : NetworkBehaviour
{
    private SphereCollider targetsCollider;
    private Dictionary<int, GameObject> currentPotentialTargets;
    private GameObject m_parentEntity;

    private GameObject currentTarget;

    // Start is called before the first frame update
    [ServerCallback]
    void Start()
    {
        currentPotentialTargets = new Dictionary<int, GameObject>();
        m_parentEntity = gameObject.transform.parent.gameObject;
        targetsCollider = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        currentTarget = null;
        float biggestThreat = -1;

        foreach (KeyValuePair<int, GameObject> it in currentPotentialTargets) {
            float playerThreat = getAvgThreat(it.Value);
            if (it.Value.GetComponent<BaseChar>().enabled && biggestThreat < playerThreat)
            {
                biggestThreat = playerThreat;
                currentTarget = it.Value;
            }
        }
    }

    [Server]
    public bool hasTarget()
    {
        return currentTarget != null;
    }

    [Server]
    public GameObject getTarget()
    {
        return (hasTarget() ? currentTarget : null);
    }

    /* --- ---------------------------------- --- */
    /* --- Evaluation des différents facteurs --- */
    /* --- ---------------------------------- --- */
    private float getAvgThreat(GameObject potentialTarget)
    {
        Vector3 factors = new Vector3(evalVisibilityFactor(potentialTarget)
                                     ,evalDistanceFactor(potentialTarget)
                                     ,evalVDamagesFactor(potentialTarget));
        return factors.magnitude;
    }

    private float evalVisibilityFactor(GameObject potentialTarget)
    {
        return 0;
    }

    private float evalDistanceFactor(GameObject potentialTarget)
    {
        float distanceToPotentialTarget = (potentialTarget.transform.position - m_parentEntity.transform.position).magnitude;
        return 1-(targetsCollider.radius / distanceToPotentialTarget);
    }

    private float evalVDamagesFactor(GameObject potentialTarget)
    {
        return 0;
    }


    /* --- ------------------------------------ --- */
    /* --- Récupération des cibles potentielles --- */
    /* --- ------------------------------------ --- */
    /* Getters*/
    public bool IsPotentialTarget(GameObject entity)
    {
        return currentPotentialTargets.ContainsKey(entity.GetInstanceID());
    }

    /* Triggers */
    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedEntity = other.gameObject;

        if (collidedEntity.tag == "Player"
            && !IsPotentialTarget(collidedEntity))
        {
            currentPotentialTargets.Add(collidedEntity.GetInstanceID(), collidedEntity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collidedEntity = other.gameObject;

        if (collidedEntity.tag == "Player"
            && IsPotentialTarget(collidedEntity))
        {
            currentPotentialTargets.Remove(collidedEntity.GetInstanceID());
        }
    }
}
