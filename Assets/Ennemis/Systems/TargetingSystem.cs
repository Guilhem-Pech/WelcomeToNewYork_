using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    private GameObject currentTarget;
    private GameObject[] players;

    public float alertDist;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearestPlayer = null;
        float nearestPlayerDistance = int.MaxValue;

        foreach (GameObject player in players) {
            float distanceToGO = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToGO < alertDist && distanceToGO < nearestPlayerDistance)
            {
                nearestPlayer = player;
                nearestPlayerDistance = distanceToGO;
            }
        }

        currentTarget = nearestPlayer;
    }

    public bool hasTarget()
    {
        return currentTarget != null;
    }

    public GameObject getTarget()
    {
        return (hasTarget() ? currentTarget : null);
    }
}
