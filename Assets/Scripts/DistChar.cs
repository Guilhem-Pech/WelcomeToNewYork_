using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DistChar : BaseChar
{
    private float nextActionTime = 0.0f;
    public float period = 1.0f;

    public GameObject[] Projectile; // tableau répértoriant les attaques du joueur
    int nextAttackID = 0; //numéro de l'attaque qui va être utiliser pour la prochaine attaque du joueur


    protected override void Attack(Vector3 point)
    {
        print("On Tir");
        GameObject currentShot = Projectile[nextAttackID]; // On récupère le prefab du projectile
        if (currentStamina >= currentShot.GetComponent<Projectile>().stamCost)
        {
            UseStamina(currentShot.GetComponent<Projectile>().stamCost); // on utilise le stamina
            Vector3 playerPosition = this.transform.position; // position du joueur
            //playerPosition.y += 1.35f; // Add the height of the character

            float angle = this.gameObject.GetComponent<PlayerAnimation>().handGameObject.transform.rotation.eulerAngles.z; // on récupère l'angle de la main pour avoir l'angle de tir

            GameObject theShot = Instantiate(currentShot); // On instantie le projectile
            NetworkServer.Spawn(theShot);
            theShot.GetComponent<Projectile>().initialisation(angle, playerPosition); // On initialise les valeurs
            RpcAttaque();
        }

    }


    [Command]
    void CmdAttaque(Vector3 hitVector)
    {
        Attack(hitVector);
    }

    [ClientRpc]
    void RpcAttaque()
    {
        playerAnimation.ShootHands(); // On lance l'animation de tir
    }


    [Command]
    void CmdAttaqueSpeciale(Vector3 playerPosplayerPos, float angle)
    {
        AttackSpeciale(playerPosplayerPos, angle);
    }

    [Command]
    void CmdTakeDamage(int dmg)
    {
        TakeDamage(dmg); ;
    }

    [Command]
    void CmdAddHealth(int health)
    {
        AddHealth(health); ;
    }



    protected override void AttackSpeciale(Vector3 playerPosition_, float angle)
    {
        print("Bonjour je suis naerly le gentil !");
    }


    public override void Update()
    {
        base.Update();
        if (isServer)
            UpdateServer();
        if (isLocalPlayer)
            UpdateClient();

    }



   [Server]
    public void UpdateServer()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            if (currentStamina != maxStamina)
            {
                GainStamina(1);
            }
        }

    }

    [Client]
    public void UpdateClient()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CmdAttaque(hit.point); //Lance le code coté client

            }

        }
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 PlayPos = this.GetComponent<BaseChar>().transform.position;
                float angleMain = this.gameObject.GetComponent<PlayerAnimation>().handGameObject.transform.rotation.eulerAngles.z; // on récupère l'angle de la main pour avoir l'angle de tir

                CmdAttaqueSpeciale(PlayPos, angleMain);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            CmdTakeDamage(50);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            CmdAddHealth(50);
        }

    }

}
