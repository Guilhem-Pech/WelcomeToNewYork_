using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    private SphereCollider attackCollider;
    private Dictionary<int, GameObject> currentAttackTargets;
    private List<GameObject> currentAttackTargetsList;
    private GameObject m_parentEntity;

    // Start is called before the first frame update
    void Awake()
    {
        currentAttackTargetsList = new List<GameObject>();
        currentAttackTargets = new Dictionary<int, GameObject>();
        m_parentEntity = gameObject.transform.parent.gameObject;
        attackCollider = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentAttackTargetsList = new List<GameObject>();
        List<int> toDeleteNeighboursList = new List<int>();

        foreach (KeyValuePair<int, GameObject> attackTarget in currentAttackTargets)
        {
            if (attackTarget.Value == null)
            {
                toDeleteNeighboursList.Add(attackTarget.Key);
            }
            else if(attackTarget.Value.GetComponent<BaseEntity>().enabled)
            {
                currentAttackTargetsList.Add(attackTarget.Value);
            }
        }

        foreach (int key in toDeleteNeighboursList)
        {
            currentAttackTargets.Remove(key);
        }
    }

    /* Getter */
    public bool IsAlreadyTarget(GameObject entity)
    {
        return currentAttackTargets.ContainsKey(entity.GetInstanceID());
    }

    public List<int> getTargeIDs()
    {
        List<int> returnList = new List<int>();

        foreach (KeyValuePair<int, GameObject> attackTarget in currentAttackTargets)
        {
            returnList.Add(attackTarget.Value.GetInstanceID());
        }

        return returnList;
    }

    public List<GameObject> getTargetList()
    {
        return currentAttackTargetsList;
    }

    public int getCount()
    {
        return currentAttackTargets.Count;
    }

    /* Triggers */
    private void OnTriggerEnter(Collider other)
    {

        GameObject collidedEntity = other.gameObject;

        if (collidedEntity.tag == "Player"
            && !IsAlreadyTarget(collidedEntity))
        {
            currentAttackTargets.Add(collidedEntity.GetInstanceID(), collidedEntity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collidedEntity = other.gameObject;

        if (collidedEntity.tag == "Player"
            && IsAlreadyTarget(collidedEntity))
        {
            currentAttackTargets.Remove(collidedEntity.GetInstanceID());
        }
    }
}

