﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float knockbackForce;
    public float knockbackDuration;


    public AudioClip launchingSound;
    public AudioClip touchSound;
    AudioSource SoundSource;
    public ParticleSystem ImpactParticle;


    public float radius;
    public float height;
    public float vitesse = 0.5f;
    public float range = 1f;
    public int stamCost = 1;

    private float exisTime;
    private Vector2 direction;
    private float distHand = 0.5f;



    public void initialisation(float angle,Vector3 startPos)
    {
        //direction = dir;

        direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * (-angle+90)), Mathf.Cos(Mathf.Deg2Rad * (-angle+90)));
        this.gameObject.GetComponent<CapsuleCollider>().radius = radius;
        this.gameObject.GetComponent<CapsuleCollider>().height = height;

        this.gameObject.transform.position= new Vector3 (startPos.x + (direction.x/1.1f),startPos.y,startPos.z + (direction.y/1.1f));

        //angle += 90f;
        /*if (dir.x > 0)
        {
            rotdeg = (Mathf.Acos(dir.y) * Mathf.Rad2Deg);
        }
        else
        {
            rotdeg = (Mathf.Asin(dir.y) * Mathf.Rad2Deg) + 90;
        };*/
        this.transform.Rotate(new Vector3(0, 0, angle), Space.Self);
        exisTime = Time.time;
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x,0f,direction.y) * vitesse);

        //Sound
        SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.clip = launchingSound;
        SoundSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Mathf.Abs(direction.x) * vitesse * (Time.time - exisTime)) + (Mathf.Abs(direction.y) * vitesse *(Time.time - exisTime)) ;
        if (distance >= range)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag != "Player" && other.gameObject.tag != "NeighboursTrigger")
        {
            StartCoroutine(DestructionProcedure(1f));
            if (other.gameObject.tag == "ennemy")
            {
                if (other.gameObject.GetComponent<TestEnnemy>() != null)
                {
                    other.gameObject.GetComponent<TestEnnemy>().onHit(direction, knockbackForce, knockbackDuration, damage);
                
                }
            }
        }
        
    }

    public IEnumerator DestructionProcedure(float duration)
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        SoundSource.clip = touchSound;
        SoundSource.Play();
        ImpactParticle.Play();

        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

}
