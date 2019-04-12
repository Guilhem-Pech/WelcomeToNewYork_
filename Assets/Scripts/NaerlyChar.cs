using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NaerlyChar : DistChar
{

    public bool AttSpeReady = true;
    public bool RechargeSpe = false;
    public float Cooldown = 1f;
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

        if (AttSpeReady)
        {
            this.GetComponent<PlayerController>().enabled = canMoveWhileAttacking;
            Vector3 playerPos = this.gameObject.transform.position; // position du joueur

            float angleSpe = this.gameObject.GetComponent<PlayerAnimation>().GetHandAngle(); // on récupère l'angle de la main pour avoir l'angle de tir
            GameObject theAttackSpe = Instantiate(AttSpe, this.gameObject.transform);
            theAttackSpe.GetComponent<NaerlyAttackSpe>().Initialisation(playerPos, angleSpe);
            NetworkServer.Spawn(theAttackSpe);

            AttSpeReady = false;
            RechargeSpe = true;
        }

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
                Cooldown = 1f;
            }
        }

    }
}
