using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MeleeAttack : NetworkBehaviour
{
    public int damage;
    public int staminaCost;
    public float duration;
    public float knockBackForce;
    public float knockBackDuration;

    public float spread;
    public float range;

    public GameObject hitBoxGO; // gameobject auquel le box collider est rattaché
    protected GameObject animationGO; // gameobject auquel l'animation est rattachée

    protected Vector3 playerPosition;
    protected Vector3 vecteurDirection; // vecteur normalisé de la direction dans laquel part l'attaque
    //protected Vector3 vecteurDirection3D;

    protected Animator animationAnimator;

    public SpriteRenderer spriteRenderer;

    protected float animationSpeed = 1f;

   
    protected BaseChar player;

    public void Initialisation(Vector3 playerPosition_, float angle)
    {        
        
        player = this.gameObject.GetComponentInParent<BaseChar> ();
        

        
        playerPosition = playerPosition_;
        vecteurDirection = new Vector3(Mathf.Cos(-angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

        this.transform.Rotate(new Vector3(0, -angle + 90, 0), Space.Self);


        hitBoxGO = this.gameObject.GetComponentInChildren<BoxCollider>().gameObject; // on récupère la hitbox de l'attaque
        hitBoxGO.transform.localScale = new Vector3(spread, 1f, range);
        this.hitBoxGO.transform.position = new Vector3((vecteurDirection.x * range / 2 + playerPosition.x), 0.5f, (vecteurDirection.z * range / 2 + playerPosition.z));
        hitBoxGO.SetActive(false);

        //Sound
               
        this.gameObject.SetActive(true);
        
    }

    [ClientRpc]
    public void RpcInitClient(GameObject p)
    {
        this.transform.SetParent(p.transform);
        this.transform.localPosition = Vector3.zero;
        player = p.GetComponent<BaseChar>();
        this.gameObject.SetActive(true);
    }

    protected void Start()
    {
        if (isServer)
        {
            player = this.gameObject.GetComponentInParent<BaseChar>();
            RpcInitClient(player.gameObject);
        }
            
        animationAnimator = this.gameObject.GetComponentInChildren<Animator>();
        animationAnimator.SetFloat("AnimationSpeedMultiplier", animationSpeed);

        player.playerAnimation.DisplayHands(false); //on cache les mains
    }

    protected void Update()
    {
        if (isServer)
            UpdateServer();
        if (isClient)
            UpdateClient();
    }
   

    void UpdateServer()
    {
        if (AnimatorIsInState("Attacking")) // L'attaque est en cours
        {
            //spriteRenderer.color = Color.red;
            hitBoxGO.SetActive(true);
        }
        else if (AnimatorIsInState("Ending")) // L'attaque est en train de se terminer
        {
            hitBoxGO.SetActive(false);
            ((MeleeChar)player).isAttacking = false;

            //spriteRenderer.color = Color.blue;
        }
        else if (AnimatorIsInState("Finished"))
        {
            //spriteRenderer.color = Color.black;
            ((MeleeChar)player).nextAttackID = 0;
            RpcFinishAttack(player.gameObject);
        }
    }


    void UpdateClient()
    {        
       /*if (AnimatorIsInState("Finished"))
        {
            
            FinishAttack();
        }*/
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!isServer)
            return;

        if (other.gameObject.tag == "ennemy")
        {
            if (other.gameObject.GetComponent<TestEnnemy>() != null)
            { 
                Vector2 knockDirection = new Vector2(other.gameObject.GetComponent<TestEnnemy>().transform.position.x - playerPosition.x, other.gameObject.GetComponent<TestEnnemy>().transform.position.z - playerPosition.z).normalized;
                // print("VDirection " + vecteurDirection + "Direction KnockBack" + knockDirection);
                other.gameObject.GetComponent<TestEnnemy>().onHit(knockDirection * knockBackForce, 500, knockBackDuration, damage);
            }
        }
    }

    protected bool AnimatorIsInState(string stateName)
    {
        return animationAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    [ClientRpc]
    protected void RpcFinishAttack(GameObject entity)
    {
        entity.GetComponent<BaseChar>().playerAnimation.DisplayHands(true); //on reaffiche les mains
        entity.GetComponent<PlayerController>().enabled = true;
        Destroy(this.gameObject);
    }

    /*[Server]
    /*protected void FinishAttack()
    {
        player.playerAnimation.DisplayHands(true); //on reaffiche les mains
        player.GetComponent<PlayerController>().enabled = true;
        Destroy(this.gameObject);
    }*/
}
