using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetingSystem : NetworkBehaviour
{
    private GameObject currentTarget;
    private GameObject[] players;

    public float alertDist;

    // Start is called before the first frame update
    [ServerCallback]
    void Start()
    {
            currentTarget = null;
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
       
            players = GameObject.FindGameObjectsWithTag("Player");

            GameObject nearestPlayer = null;
            float nearestPlayerDistance = int.MaxValue;

            foreach (GameObject player in players)
            {
                float distanceToGO = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToGO < alertDist && distanceToGO < nearestPlayerDistance)
                {
                    nearestPlayer = player;
                    nearestPlayerDistance = distanceToGO;
                }
            }

            currentTarget = nearestPlayer;
        
        
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
}
