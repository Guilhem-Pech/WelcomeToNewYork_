using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int damage;
    public int staminaCost;
    public float duration;
    public float knockBackForce;

    public float spread;
    public float range;

    BoxCollider boxCollider;

    public AudioClip sound;
    public float animationDuration;

    Vector3 playerPosition;
    Vector3 vecteurDirection; // vecteur normalisé de la direction dans laquel part l'attaque


    public void Initialisation(Vector3 playerPosition_, float angle)
    {
        playerPosition = playerPosition_;
        vecteurDirection = new Vector3(Mathf.Cos(-angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

        //Definir Position, Rotation, BoxCollider (taille)
        this.transform.position = new Vector3((vecteurDirection.x * range / 2 + playerPosition.x), 1.5f, (vecteurDirection.z * range / 2 + playerPosition.z));
        /*float rotdeg;
        if (vecteurDirection.x > 0)
        {
            rotdeg = Mathf.Acos(vecteurDirection.y) * Mathf.Rad2Deg;
        }
        else
        {
            rotdeg = Mathf.Asin(vecteurDirection.y) * Mathf.Rad2Deg - 90;
        };*/
        this.transform.Rotate(new Vector3(0, -angle+90, 0), Space.Self);

        boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        //boxCollider.size = new Vector3(spread, 1f, range);
        this.transform.localScale = new Vector3(spread, 1f, range);

        //Sound
        AudioSource SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.clip = sound;
        SoundSource.Play();

        //Animation


        StartCoroutine(Duration()); // Launch the attack
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(animationDuration);
        Destroy(this.gameObject);
        this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = false ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ennemy")
        {
            if (other.gameObject.GetComponent<TestEnnemy>() != null)
            { 
                Vector2 knockDirection = new Vector2(other.gameObject.GetComponent<TestEnnemy>().transform.position.x - playerPosition.x, other.gameObject.GetComponent<TestEnnemy>().transform.position.z - playerPosition.z).normalized;
                print("VDirection " + vecteurDirection + "Direction KnockBack" + knockDirection);
                    
                other.gameObject.GetComponent<TestEnnemy>().onHit(knockDirection * knockBackForce, 500, 0.75f, damage);
            }
        }
    }
}
