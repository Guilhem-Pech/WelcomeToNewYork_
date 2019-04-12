using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class NaerlyAttackSpe : NetworkBehaviour
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

    protected Animator animationAnimator;

    private NaerlyChar player;

    public SpriteRenderer spriteRenderer;

    protected float animationSpeed = 1f;

    public void Initialisation(Vector3 playerPosition_, float angle)
    {
        player = this.gameObject.GetComponentInParent<NaerlyChar>();
        playerPosition = playerPosition_;
        vecteurDirection = new Vector3(Mathf.Cos(-angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

        this.transform.Rotate(new Vector3(0, -angle + 90, 0), Space.Self);
        
        hitBoxGO = this.gameObject.GetComponentInChildren<BoxCollider>().gameObject; // on récupère la hitbox de l'attaque
        hitBoxGO.transform.localScale = new Vector3(spread, 1f, range);
        this.hitBoxGO.transform.position = new Vector3((vecteurDirection.x * range / 2 + playerPosition.x), 0.5f, (vecteurDirection.z * range / 2 + playerPosition.z));
        hitBoxGO.SetActive(false);
        this.gameObject.SetActive(true);

    }

    [ClientRpc]
    public void RpcInitClient(GameObject p)
    {
        this.transform.SetParent(p.transform);
        player = p.GetComponent<NaerlyChar>();
        this.gameObject.SetActive(true);
    }

    protected void Start()
    {
        if (isServer)
        {
            player = this.gameObject.GetComponentInParent<NaerlyChar>();
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

    private void UpdateServer()
    {
        if (AnimatorIsInState("Startup"))
        {
            this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.ShowSpecialSprite(true, this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.handClothesSpriteRenderer.flipY);
        }
        else if (AnimatorIsInState("Attacking")) // L'attaque est en cours
        {
            hitBoxGO.SetActive(true);
        }
        else if (AnimatorIsInState("Ending")) // L'attaque est en train de se terminer
        {
            NavMeshHit hit;
            if (NavMesh.Raycast(new Vector3(playerPosition.x, 0.5f, playerPosition.z), new Vector3((vecteurDirection.x * range + playerPosition.x), 0.5f, (vecteurDirection.z * range + playerPosition.z)), out hit, NavMesh.AllAreas))
            {
                player.transform.position = hit.position;
            }
            else
            {
                player.transform.position = new Vector3((vecteurDirection.x * range + playerPosition.x), 0, (vecteurDirection.z * range + playerPosition.z));
            }

            hitBoxGO.SetActive(false);
        }
        else if (AnimatorIsInState("Finished"))
        {
            //reafficher le personnage aussi
            FinishAttack();
        }
    }



    private void UpdateClient()
    {
        if (AnimatorIsInState("Startup")) // L'attaque commence
        {
            this.GetComponentInParent<PlayerController>().enabled = false;
            this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.DisplayHands(false); //on cache les mains
                                                                                                   //cacher le personnage aussi
            this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.ShowSpecialSprite(true, this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.handClothesSpriteRenderer.flipY);
        }
        
        else if (AnimatorIsInState("Finished"))
        {
            //reafficher le personnage aussi
            FinishAttack();
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        Vector3 dir;
        if (other.gameObject.tag == "ennemy")
        {
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

    protected bool AnimatorIsInState(string stateName)
    {
        return animationAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    protected void FinishAttack()
    {      
        player.playerAnimation.DisplayHands(true); //on reaffiche les mains
        player.GetComponent<PlayerController>().enabled = true;
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.ShowSpecialSprite(false, this.gameObject.GetComponentInParent<NaerlyChar>().playerAnimation.handClothesSpriteRenderer.flipY);
    }
}
