using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JojoChar : MeleeChar
{
    
    

    [ServerCallback]
    public override void Awake()
    {
        cooldown = 4f;
        period = 0.001f;
        maxHealth = 250;
        maxStamina = 75;
        nextAttackID = 0;
    }

    [ServerCallback]
    public override void Start()
    {
        base.Start();
    }

    protected override void AttackSpeciale(Vector3 playerPosition_, float angle)
    {
        if (attSpeReady && !isAttacking)
        {
            foreach (MeleeAttack aMeleeAttack in this.gameObject.GetComponentsInChildren<MeleeAttack>())
                Destroy(aMeleeAttack.gameObject);
            this.GetComponent<PlayerController>().enabled = canMoveWhileAttacking;
            isAttacking = true;
            Vector3 playerPos = this.gameObject.transform.position; // position du joueur

            float angleSpe = this.gameObject.GetComponent<PlayerAnimation>().GetHandAngle(); // on récupère l'angle de la main pour avoir l'angle de tir
            GameObject theAttackSpe = Instantiate(attSpe, this.gameObject.transform);
            theAttackSpe.GetComponent<JojoAttackSpe>().Initialisation(playerPos, angleSpe);
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
                cooldown = tpsRecharge;
            }
        }

    }
}
