using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NaerlyChar : DistChar
{

    public bool AttSpeReady = true;
    public bool RechargeSpe = false;
    public float Cooldown = 0.001f;
    public GameObject AttSpe;

    [ServerCallback]
    public override void Awake()
    {
        period = 0.001f;
        maxHealth = 100;
        maxStamina = 75;
    }

    [ServerCallback]
    public override void Start()
    {
        base.Start();
    }

    protected override void AttackSpeciale(Vector3 playerPosition_, float angle)
    {
        

    }

    private new void Update()
    {
        base.Update();

        if (RechargeSpe)
        {
            Cooldown -= Time.deltaTime;
            if (Cooldown <= 0)
            {
                RechargeSpe = false;
                AttSpeReady = true;
                Cooldown = 0.001f;
            }
        }

    }
}
