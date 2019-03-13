﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(PlayerController))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Base")]
    public SpriteRenderer bodySpriteRenderer;
    public Animator bodyAnimator;
    public SpriteRenderer headSpriteRenderer;
    public Animator headAnimator;
    public SpriteRenderer handSpriteRenderer;
    //public Animator handAnimator;

    [Header("Clothes")]
    public SpriteRenderer bodyClothesSpriteRenderer;
    public Animator bodyClothesAnimator;
    public SpriteRenderer headClothesSpriteRenderer;
    public Animator headClothesAnimator;

    public SpriteRenderer handClothesSpriteRenderer;
    //public Animator handClothesAnimator;

    [Header("Hand")]
    public GameObject handGameObject;

    [Header("Dust")]
    public ParticleSystem dustParticleSystem;

    Vector3 moveDirection;
    float moveSpeed = 0f;

    bool idle = true;


    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
     playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 moveDirection = playerController.getMoveDirection(); //new Vector3(horizontalDirection, 0 , verticalDirection);
        moveDirection = moveDirection.normalized;
        moveSpeed = (moveDirection.normalized.magnitude) * 6f;
        //a supprimer lorsque j'aurais les valeurs de movespeed

        if (moveDirection != new Vector3(0, 0, 0)) // si on se déplace
        {
            if (idle) // si on vient de commencer à se déplacer
            {
                dustParticleSystem.Play(true);
                idle = false;
            }
            DustEffect(dustParticleSystem, moveDirection);
        }
        else // si on se déplace pas
        {
            if (!idle) // si on vient de s'arrêter de se déplacer
            {
                dustParticleSystem.Stop(true);
                idle = true;
            }

        }

        AllAnimatorSetFloat("Speed", moveSpeed);

        if (moveDirection.x > 0)
        {
            bodySpriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            bodySpriteRenderer.flipX = true;
        }

        /*if (!isLocalPlayer)
            return;
            */

        Vector2 mousePosition = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (hit.point.x > this.gameObject.transform.position.x)
            {
                headSpriteRenderer.flipX = false;
                handClothesSpriteRenderer.flipY = false;
            }
            else
            {
                headSpriteRenderer.flipX = true;
                handClothesSpriteRenderer.flipY = true;
            }

            /*
            //Si on vise vers le haut, la main passe derrière le personnage
            if (hit.point.z > this.gameObject.transform.position.z)
            {
                handClothesSpriteRenderer.gameObject.transform.localPosition = new Vector3(handClothesSpriteRenderer.gameObject.transform.localPosition.x, handClothesSpriteRenderer.gameObject.transform.localPosition.y, -0.004f);
            }
            else
            {
                handClothesSpriteRenderer.gameObject.transform.localPosition = new Vector3(handClothesSpriteRenderer.gameObject.transform.localPosition.x, handClothesSpriteRenderer.gameObject.transform.localPosition.y, -0.004f);
            }
            */ //Ne Marche pas pour l'instant

            Vector3 playerPosition = this.gameObject.transform.position;
            Vector3 dir = hit.point - playerPosition;
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.z+dir.y, dir.x) * Mathf.Rad2Deg;
            handGameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }

        ClothesUpdate();
    }

    void ClothesUpdate()
    {
        bodyClothesSpriteRenderer.flipX = bodySpriteRenderer.flipX;
        headClothesSpriteRenderer.flipX = headSpriteRenderer.flipX;
        handClothesSpriteRenderer.flipX = handSpriteRenderer.flipX;
    }

    void DownAnimatorPlay(string stateName)
    {
        bodyAnimator.Play(stateName);
        bodyClothesAnimator.Play(stateName);
    }

    void UpAnimatorPlay(string stateName)
    {
        headAnimator.Play(stateName);
        headClothesAnimator.Play(stateName);
    }

    void AllAnimatorPlay(string stateName)
    {
        DownAnimatorPlay(stateName);
        UpAnimatorPlay(stateName);
    }

    void DownAnimatorSetFloat(string parameterName, float value)
    {
        bodyAnimator.SetFloat(parameterName, value);
        bodyClothesAnimator.SetFloat(parameterName, value);
    }

    void DownAnimatorSetBool(string parameterName, bool value)
    {
        bodyAnimator.SetBool(parameterName, value);
        bodyClothesAnimator.SetBool(parameterName, value);
    }

    void UpAnimatorSetFloat(string parameterName, float value)
    {
        headAnimator.SetFloat(parameterName, value);
        headClothesAnimator.SetFloat(parameterName, value);
    }

    void UpAnimatorSetBool(string parameterName, bool value)
    {
        headAnimator.SetBool(parameterName, value);
        headClothesAnimator.SetBool(parameterName, value);
    }

    void AllAnimatorSetFloat(string parameterName, float value)
    {
        DownAnimatorSetFloat(parameterName, value);
        UpAnimatorSetFloat(parameterName, value);
    }

    void AllAnimatorSetBool(string parameterName, bool value)
    {
        DownAnimatorSetBool(parameterName, value);
        UpAnimatorSetBool(parameterName, value);
    }

    void DustEffect(ParticleSystem particleSystem, Vector3 moveDirection_)
    {
        var shape = particleSystem.shape;
        Vector3 moveDirection = moveDirection_;
        Vector3 shapeRotation;

        shapeRotation = new Vector3(0, Vector2DToAngle(moveDirection.x, -moveDirection.z), 0);
        shape.rotation = shapeRotation;

        ParticleSystemRenderer pr = particleSystem.GetComponent<ParticleSystemRenderer>();
        if (moveDirection.z > -1)
        {
            pr.sortingOrder = 10;
        }
        else
        {
            pr.sortingOrder = 0;
        }

    }

    float Vector2DToAngle(float x, float y)
    {
        var rad = Mathf.Atan(y / x);   // arcus tangent in radians
        var deg = rad * 180 / Mathf.PI;  // converted to degrees
        if (x < 0) deg += 180;        // fixed mirrored angle of arctan
        var eul = (270 + deg) % 360;    // folded to [0,360) domain
        return eul;
    }

    //Fait disparaitre les mains et les fait reapparaitre après "duration" secondes
    public void DisplayHands(bool isHandDisplayed)
    {
        handClothesSpriteRenderer.enabled = isHandDisplayed;
        handSpriteRenderer.enabled = isHandDisplayed;
    }


    public void ShootHands()
    {
        handClothesSpriteRenderer.gameObject.GetComponent<Animator>().Play("Shoot");
        handClothesSpriteRenderer.gameObject.GetComponent<Animator>().CrossFadeInFixedTime("Shoot", 0f);
    }



}
