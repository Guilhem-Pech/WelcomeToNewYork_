using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JojoAttackSpe : MeleeAttack
{
    protected new void Update()
    {
        if (isServer)
            UpdateServer();

        if (isClient)
            UpdateClient();

    }


    private void UpdateServer()
    {
        if (AnimatorIsInState("Startup"))
        {
            this.GetComponentInParent<PlayerController>().enabled = false;
            this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.DisplayHands(false); //on cache les mains
            this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = true;
            this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.ShowSpecialSprite(true, this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.handClothesSpriteRenderer.flipY);
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
            RpcFinishAttack(player.gameObject);
        }
    }



    private void UpdateClient()
    {
        if (AnimatorIsInState("Startup")) // L'attaque commence
        {
            this.GetComponentInParent<PlayerController>().enabled = false;
            this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.DisplayHands(false); //on cache les mains
                                                                                                   //cacher le personnage aussi
            this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.ShowSpecialSprite(true, this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.handClothesSpriteRenderer.flipY);
            //this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = true;
        }
        else if (AnimatorIsInState("Finished"))
        {
            //spriteRenderer.color = Color.black;
            //reafficher le personnage aussi

           // this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = false;
            //FinishAttack();
        }
    }

    [ServerCallback]
    private new void OnTriggerEnter(Collider other)
    {

        // print(vecteurDirection);
        Vector3 dir;
        if (other.gameObject.tag == "ennemy")
        {
           // // print(this.transform.position);

            Transform test = this.transform;
            Vector3 PosRelative = test.InverseTransformPoint(other.transform.position);

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

    private void OnDestroy()
    {
        this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.ShowSpecialSprite(false, this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.handClothesSpriteRenderer.flipY);
    }

}
