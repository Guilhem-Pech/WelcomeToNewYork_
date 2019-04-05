using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AttackSystem : NetworkBehaviour
{
    public bool CanEscape;
    public float escapeTriggerDistance;
    public float escapeRange;

    public AbstractAttack attackAbility;
    private Animator animController;
    private SphereCollider attackCollider;
    private Dictionary<int, GameObject> currentAttackTargets;
    private List<GameObject> currentAttackTargetsList;
    private GameObject m_parentEntity;

    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
        currentAttackTargetsList = new List<GameObject>();
        currentAttackTargets = new Dictionary<int, GameObject>();
        animController = transform.parent.GetComponent<Animator>();
        m_parentEntity = gameObject.transform.parent.gameObject;
        attackCollider = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    [ServerCallback]
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
        return entity != null && currentAttackTargets.ContainsKey(entity.GetInstanceID());
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
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {

        GameObject collidedEntity = other.gameObject;

        if (collidedEntity.tag == "Player"
            && !IsAlreadyTarget(collidedEntity))
        {
            currentAttackTargets.Add(collidedEntity.GetInstanceID(), collidedEntity);
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        GameObject collidedEntity = other.gameObject;

        if (collidedEntity.tag == "Player"
            && IsAlreadyTarget(collidedEntity))
        {
            currentAttackTargets.Remove(collidedEntity.GetInstanceID());
        }
    }

    [Server]
    public bool IsTargetTooClose(GameObject target)
    {
        if (!CanEscape || target == null)
            return false;
        return ((target.transform.position - gameObject.transform.position).magnitude <= escapeTriggerDistance);
    }

    [Server]
    public void AnimAttackLaunch()
    {
        animController.GetBehaviour<AttackBehaviour>().OnAnimAttackLaunch();
    }
    [Server]
    public void AnimAttackEnd()
    {
        animController.GetBehaviour<AttackBehaviour>().OnAnimAttackEnd();
    }

    [Server]
    public void AnimTeleportLaunch()
    {
        animController.GetBehaviour<EscapeBehaviour>().OnAnimTeleportLaunch();
    }
    [Server]
    public void AnimTeleportEnd()
    {
        animController.GetBehaviour<EscapeBehaviour>().OnAnimTeleportEnd();
    }
}

