using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class MeleeChar : BaseChar
{
    private float nextActionTime = 0.0f;
    [SyncVar]
    public bool isAttacking = false ;
    public float period = 1.0f;

    public List<AudioClip> attack;
    public AudioClip special;

    public GameObject[] Attacks; // tableau répértoriant les attaques du joueur
    //[SyncVar]
    public int nextAttackID = 0; //numéro de l'attaque qui va être utiliser pour la prochaine attaque du joueur

    protected override void Attack(Vector3 point)
    {
        
        GameObject currentAttack = Attacks[nextAttackID]; // On récupère le prefab de l'attaque
        if (currentStamina >= currentAttack.GetComponent<MeleeAttack>().staminaCost)
        {
            if (attack.Count != 0)
            {
                int randomSound = Random.Range(0, attack.Count);
                SoundManager.instance.PlaySound(attack[randomSound], this.gameObject);
            }


            foreach (MeleeAttack aMeleeAttack in this.gameObject.GetComponentsInChildren<MeleeAttack>())
                Destroy(aMeleeAttack.gameObject);
            this.GetComponent<PlayerController>().enabled = canMoveWhileAttacking;
            isAttacking = true;
            nextAttackID = (nextAttackID + 1) % 3;
            print("NextAttackID: " + nextAttackID);

            UseStamina(currentAttack.GetComponent<MeleeAttack>().staminaCost);
            Vector3 playerPos = this.gameObject.transform.position; // position du joueur

            float angle = this.gameObject.GetComponent<PlayerAnimation>().GetComponent<PlayerAnimation>().GetHandAngle(); // on récupère l'angle de la main pour avoir l'angle de tir


            GameObject theAttack = Instantiate(currentAttack, this.gameObject.transform);
            theAttack.GetComponent<MeleeAttack>().Initialisation(playerPos, angle);
            NetworkServer.SpawnWithClientAuthority(theAttack, this.gameObject);
        }
        else
        {
            isAttacking = false;
        }

    }

    [Command]
    void CmdAttaque(Vector3 hitVector)
    {
        Attack(hitVector);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();


        if (isServer)
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


        if (!isLocalPlayer)
            return;

        if (Input.GetButton("Fire1") && !isAttacking)
        {
            RaycastHit hit ;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                isAttacking = true;
                CmdAttaque(hit.point);
            }

        }

            if (Input.GetButtonDown("Fire2") && !isAttacking)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 PlayPos = this.GetComponent<BaseChar>().transform.position;
                float angleMain = this.gameObject.GetComponent<PlayerAnimation>().GetHandAngle(); // on récupère l'angle de la main pour avoir l'angle de tir

                CmdAttaqueSpe(PlayPos, angleMain);
            }
        }
    }

    [Command]
    public void CmdAttaqueSpe(Vector3 playerPos, float angle)
    {
        AttackSpeciale(playerPos, angle);
    }

}
