using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AjirogChar : DistChar
{
    
    public int shotgunAmmo = 5;
    

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
        if (attSpeReady)
        {
            this.lastShot = Time.time;
            GameObject currentShot = Projectile[1]; // On récupère le prefab du projectile
            shake = 1;
            if (currentSpread < spreadMax)
                currentSpread += 0.5f;
            else
                currentSpread = spreadMax;

            //UseStamina(currentShot.GetComponent<Projectile>().stamCost); // on utilise le stamina
            Vector3 playerPosition = this.transform.position; // position du joueur
            //playerPosition.y += 1.35f; // Add the height of the character

            angle -= 20;
            for(var i = 0; i < shotgunAmmo; i++)
            { 
                GameObject theShot = Instantiate(currentShot); // On instantie le projectile
                NetworkServer.Spawn(theShot);
                float randAngle = Random.Range(angle - (currentSpread / 2), angle + (currentSpread / 2));
                theShot.GetComponent<Projectile>().initialisation(randAngle, playerPosition); // On initialise les valeurs
                RpcAttaqueSpecial();
                angle += 40/shotgunAmmo;
            }
            

           
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
