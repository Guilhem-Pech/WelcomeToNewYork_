using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiParams : Singleton<EnnemiParams>
{
    public float SteeringUpdateCooldown = 0.1f;

    /* --- Param Speeds --- */
    public float ChaseSpeed = 3.5f;
    public float WanderSpeed = 1.5f;

    /* --- Param comportement Général --- */
    public float ProximityDistance = 1f;

    /* --- Param comportement idle --- */
    public float MinIntervalTillWander = 2.5f;

    /* --- Param comportement wander --- */
    public float WanderJitter = 70f;
    public float WanderRadius = 10;

    /* --- Poids des comportements --- */
    public float WeightSeparation = 1.2f;
    public float WeightCohesion = 2f;
    public float WeightAlignment = 1.3f;
    public float WeightWander = 1f;
    public float WeightSeek = 1f;
    public float WeightFlee = 1;
    public float WeightPursuit = 1;
    public float WeightOffsetPursuit = 1;
    public float WeightInterpose = 1;
    public float WeightHide = 1;
    public float WeightEvade = 1;
}
