using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeChar : BaseChar
{
    private float nextActionTime = 0.0f;
    public bool isAttacking = false ;
    public float period = 1.0f;

    public GameObject[] Attacks; // tableau répértoriant les attaques du joueur
    int nextAttackID = 0; //numéro de l'attaque qui va être utiliser pour la prochaine attaque du joueur

    protected override void attack(Vector3 point)
    {
        
        GameObject currentAttack = Attacks[nextAttackID]; // On récupère le prefab de l'attaque
        if (currentStamina >= currentAttack.GetComponent<MeleeAttack>().staminaCost )
        {
            isAttacking = true;
            print("Clic du joueur " +point);
            useStamina(currentAttack.GetComponent<MeleeAttack>().staminaCost);
            Vector3 playerPos = this.gameObject.transform.position; // position du joueur
            Vector2 vecteurRangeNorm; // vecteur partant du joueur et allant vers le point cliqué
            vecteurRangeNorm.x = point.x - playerPos.x;
            vecteurRangeNorm.y = point.z - playerPos.z;
            vecteurRangeNorm.Normalize() ;

            float angle = this.gameObject.GetComponent<PlayerAnimation>().handGameObject.transform.rotation.eulerAngles.z; // on récupère l'angle de la main pour avoir l'angle de tir


            GameObject theAttack = Instantiate(currentAttack, this.gameObject.transform);
            theAttack.GetComponent<MeleeAttack>().Initialisation(playerPos, angle);
            StartCoroutine(playerAnimation.DisableEnableHands(theAttack.GetComponent<MeleeAttack>().animationDuration));

        }

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Fire1") && ! isAttacking)
        {
            RaycastHit hit ;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                attack(hit.point);
            }
            
        }
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 PlayPos = this.GetComponent<BaseChar>().transform.position;
                Vector2 hitPoint = new Vector2(hit.point.x - PlayPos.x, hit.point.z - PlayPos.z);

                AttackSpeciale(PlayPos, hitPoint);
            }
        }
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            if (currentStamina != maxStamina)
            {
                gainStamina(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            takeDamage(50);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            getHeal(50);
        }


    }

}
