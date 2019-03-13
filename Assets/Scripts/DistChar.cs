using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistChar : BaseChar
{
    private float nextActionTime = 0.0f;
    public float period = 1.0f;

    public GameObject[] Projectile; // tableau répértoriant les attaques du joueur
    int nextAttackID = 0; //numéro de l'attaque qui va être utiliser pour la prochaine attaque du joueur


    protected override void attack(Vector3 point)
    {
        print("On Tir");
        GameObject currentShot = Projectile[nextAttackID]; // On récupère le prefab du projectile
        if (currentStamina >= currentShot.GetComponent<Projectile>().stamCost)
        {
            useStamina(currentShot.GetComponent<Projectile>().stamCost); // on utilise le stamina
            Vector3 playerPosition = this.transform.position; // position du joueur
            //playerPosition.y += 1.35f; // Add the height of the character

            float angle = this.gameObject.GetComponent<PlayerAnimation>().handGameObject.transform.rotation.eulerAngles.z; // on récupère l'angle de la main pour avoir l'angle de tir

            GameObject theShot = Instantiate(currentShot); // On instantie le projectile
            theShot.GetComponent<Projectile>().initialisation(angle, playerPosition); // On initialise les valeurs
            playerAnimation.ShootHands(); // On lance l'animation de tir

        }

    }

    protected override void AttackSpeciale(Vector3 playerPosition_, Vector2 vecteurDirection_)
    {
        print("Bonjour je suis naerly le gentil !");
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
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
