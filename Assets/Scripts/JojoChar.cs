using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JojoChar : MeleeChar
{

    public bool AttSpeReady = true;
    public bool RechargeSpe = false;
    public float Cooldown = 4f;
    public GameObject AttSpe;

    [ServerCallback]
    public override void Awake()
    {
        period = 0.001f;
        maxHealth = 250;
        maxStamina = 75;
    }

    [ServerCallback]
    public override void Start()
    {
        base.Start();
    }

    protected override void AttackSpeciale(Vector3 playerPosition_, float angle)
    {
        /*if (AttSpeReady)
        {
            GameObject theAttack = Instantiate(AttSpe, this.gameObject.transform);
            theAttack.GetComponent<JojoAttackSpe>().AttackSpe(playerPosition_, angle);
            //StartCoroutine(playerAnimation.DisableEnableHands(theAttack.GetComponent<MeleeAttack>().animationDuration));

            AttSpeReady = false;
            RechargeSpe = true;
        }*/

        if (AttSpeReady)
        {
            this.GetComponent<PlayerController>().enabled = canMoveWhileAttacking;
            isAttacking = true;
            Vector3 playerPos = this.gameObject.transform.position; // position du joueur

            float angleSpe = this.gameObject.GetComponent<PlayerAnimation>().handGameObject.transform.rotation.eulerAngles.z; // on récupère l'angle de la main pour avoir l'angle de tir
            GameObject theAttackSpe = Instantiate(AttSpe, this.gameObject.transform);            
            theAttackSpe.GetComponent<JojoAttackSpe>().Initialisation(playerPos, angleSpe);
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
                Cooldown = 4f;
            }
        }

    }
}
