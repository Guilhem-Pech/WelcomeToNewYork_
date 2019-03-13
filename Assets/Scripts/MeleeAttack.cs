using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
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

    public AudioClip sound;

    protected Vector3 playerPosition;
    protected Vector3 vecteurDirection; // vecteur normalisé de la direction dans laquel part l'attaque

    protected Animator animationAnimator;

    public SpriteRenderer spriteRenderer;

    protected float animationSpeed = 1f;

    public void Initialisation(Vector3 playerPosition_, float angle)
    {
        playerPosition = playerPosition_;
        vecteurDirection = new Vector3(Mathf.Cos(-angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));


        this.transform.Rotate(new Vector3(0, -angle + 90, 0), Space.Self);


        hitBoxGO = this.gameObject.GetComponentInChildren<BoxCollider>().gameObject; // on récupère la hitbox de l'attaque
        hitBoxGO.transform.localScale = new Vector3(spread, 1f, range);
        this.hitBoxGO.transform.position = new Vector3((vecteurDirection.x * range / 2 + playerPosition.x), 1.5f, (vecteurDirection.z * range / 2 + playerPosition.z));
        hitBoxGO.SetActive(false);

        //Sound
        AudioSource SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.clip = sound;
        SoundSource.Play();

        this.gameObject.SetActive(true);
    }

    protected void Start()
    {
        animationAnimator = this.gameObject.GetComponentInChildren<Animator>();
        animationAnimator.SetFloat("AnimationSpeedMultiplier", animationSpeed);

        this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.DisplayHands(false); //on cache les mains
    }

    protected void Update()
    {
        if (AnimatorIsInState("Startup")) // L'attaque commence
        {
            //spriteRenderer.color = Color.yellow;

        }
        else if (AnimatorIsInState("Attacking")) // L'attaque est en cours
        {
            //spriteRenderer.color = Color.red;
            hitBoxGO.SetActive(true);
        }
        else if (AnimatorIsInState("Ending")) // L'attaque est en train de se terminer
        {
            hitBoxGO.SetActive(false);
            this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = false;

            //spriteRenderer.color = Color.blue;
        }
        else if (AnimatorIsInState("Finished"))
        {
            //spriteRenderer.color = Color.black;
            FinishAttack();
            this.gameObject.GetComponentInParent<MeleeChar>().nextAttackID = 0;
        }

    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ennemy")
        {
            if (other.gameObject.GetComponent<TestEnnemy>() != null)
            { 
                Vector2 knockDirection = new Vector2(other.gameObject.GetComponent<TestEnnemy>().transform.position.x - playerPosition.x, other.gameObject.GetComponent<TestEnnemy>().transform.position.z - playerPosition.z).normalized;
                print("VDirection " + vecteurDirection + "Direction KnockBack" + knockDirection);
                    
                other.gameObject.GetComponent<TestEnnemy>().onHit(knockDirection * knockBackForce, 500, knockBackDuration, damage);
            }
        }
    }

    protected bool AnimatorIsInState(string stateName)
    {
        return animationAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    protected void FinishAttack()
    {
        this.gameObject.GetComponentInParent<MeleeChar>().playerAnimation.DisplayHands(true); //on reaffiche les mains
        this.gameObject.GetComponentInParent<MeleeChar>().GetComponent<PlayerController>().enabled = true;
        Destroy(this.gameObject);
    }
}
