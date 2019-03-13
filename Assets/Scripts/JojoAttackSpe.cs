using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JojoAttackSpe : MeleeAttack
{
    protected new void Update()
    {
        if (AnimatorIsInState("Startup")) // L'attaque commence
        {
            this.GetComponentInParent<PlayerController>().enabled = false;
            this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.DisplayHands(false); //on cache les mains
                                                                                                   //cacher le personnage aussi
        }
        else if (AnimatorIsInState("Attacking")) // L'attaque est en cours
        {
            //spriteRenderer.color = Color.red;
            hitBoxGO.SetActive(true);
        }
        else if (AnimatorIsInState("Ending")) // L'attaque est en train de se terminer
        {
            hitBoxGO.SetActive(false);
            //spriteRenderer.color = Color.blue;
        }
        else if (AnimatorIsInState("Finished"))
        {
            //spriteRenderer.color = Color.black;
            //reafficher le personnage aussi

            this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = false;
            FinishAttack();
        }

    }

    private new void OnTriggerEnter(Collider other)
    {
        Vector3 dir;
        if (other.gameObject.tag == "ennemy")
        {
            print(this.transform.position);

            Transform test = this.transform;
            Vector3 PosRelative = test.InverseTransformPoint(other.transform.position);

            vecteurDirection = new Vector3(vecteurDirection.x, 0, vecteurDirection.y);

            if (PosRelative.x > 0)
            {
                dir = -Vector3.Cross(vecteurDirection, new Vector3(0, 1, 0)).normalized;
            }
            else
            {
                dir = Vector3.Cross(vecteurDirection, new Vector3(0, 1, 0)).normalized;
            }

            if (other.gameObject.GetComponent<TestEnnemy>() != null)
            {
                other.gameObject.GetComponent<TestEnnemy>().onHit(new Vector2(dir.x, dir.z) * knockBackForce, 500, knockBackDuration, damage);
            }
        }
    }
}
