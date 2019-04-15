using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NaerlyChar : DistChar
{

    [ServerCallback]
    public override void Awake()
    {
        cooldown = 0.001f;
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

        if (attSpeReady)
        {
            this.GetComponent<PlayerController>().enabled = canMoveWhileAttacking;
            Vector3 playerPos = this.gameObject.transform.position; // position du joueur

            float angleSpe = this.gameObject.GetComponent<PlayerAnimation>().GetHandAngle(); // on récupère l'angle de la main pour avoir l'angle de tir
            GameObject theAttackSpe = Instantiate(attSpe, this.gameObject.transform);
            theAttackSpe.GetComponent<NaerlyAttackSpe>().Initialisation(playerPos, angleSpe);
            NetworkServer.Spawn(theAttackSpe);

            attSpeReady = false;
            rechargeSpe = true;
        }

    }

    private new void Update()
    {
        base.Update();

        if (rechargeSpe)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                rechargeSpe = false;
                attSpeReady = true;
                cooldown = 1f;
            }
        }

    }
}
