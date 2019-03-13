using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JojoChar : MeleeChar
{

    public bool AttSpeReady = true;
    public bool RechargeSpe = false;
    public float Cooldown = 10f;
    public GameObject AttSpe;


    public new void Start()
    {

        
        period = 0.5f;
        maxHealth = 250;
        maxStamina = 75;
        base.Start();

    }

    protected override void AttackSpeciale(Vector3 playerPosition_, Vector2 vecteurDirection_)
    {
        if (AttSpeReady)
        {
            GameObject theAttack = Instantiate(AttSpe, this.gameObject.transform);
            theAttack.GetComponent<JojoAttackSpe>().AttackSpe(playerPosition_, vecteurDirection_);
            //StartCoroutine(playerAnimation.DisableEnableHands(theAttack.GetComponent<MeleeAttack>().animationDuration));

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
                Cooldown = 10f;
            }
        }

    }
}
