using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JojoAttackSpe : MonoBehaviour
{

    public int damageSpe = 15;

    public float duration;
    public float knockBackForce = 5000f;
    public float knockBackDuration = 1f;

    public float spread = 1f;
    public float range = 5f;

    BoxCollider boxCollider;

    public AudioClip soundSpe;
    public float animationDuration;


    Vector3 playerPosition;
    Vector3 vecteurDirection; // vecteur normalisé de la direction dans laquel part l'attaque

    public void AttackSpe(Vector3 playerPosition_, Vector2 vecteurDirection_)
    {
        this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = true;
        this.gameObject.GetComponentInParent<PlayerController>().enabled = false;




        playerPosition = playerPosition_;
        vecteurDirection = vecteurDirection_.normalized;

        //Definir Position, Rotation, BoxCollider (taille)
        this.transform.position = new Vector3((vecteurDirection.x * range / 2 + playerPosition.x), 1.0f, (vecteurDirection.y * range / 2 + playerPosition.z));
        float rotdeg;
        if (vecteurDirection.x > 0)
        {
            rotdeg = Mathf.Acos(vecteurDirection.y) * Mathf.Rad2Deg;
        }
        else
        {
            rotdeg = Mathf.Asin(vecteurDirection.y) * Mathf.Rad2Deg - 90;
        };
        this.transform.Rotate(new Vector3(0, rotdeg, 0), Space.Self);

        boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.GetComponent<MeshRenderer>().enabled = true;
        boxCollider.isTrigger = true;
        //boxCollider.size = new Vector3(spread, 1f, range);
        this.transform.localScale = new Vector3(spread, 1f, range);


        //Sound
        AudioSource SoundSourceSpe = gameObject.AddComponent<AudioSource>();
        SoundSourceSpe.clip = soundSpe;
        SoundSourceSpe.Play();



        //Animation
        StartCoroutine(Duration()); // Launch the attack
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
        this.gameObject.GetComponentInParent<MeleeChar>().isAttacking = false;
        this.gameObject.GetComponentInParent<PlayerController>().enabled = true;

    }


    private void OnTriggerEnter(Collider other)
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
                other.gameObject.GetComponent<TestEnnemy>().onHit(new Vector2(dir.x, dir.z) * knockBackForce, 500, knockBackDuration, damageSpe);
            }
        }
    }
}
