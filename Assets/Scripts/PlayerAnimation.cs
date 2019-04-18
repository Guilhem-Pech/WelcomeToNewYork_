using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(PlayerController))]
public class PlayerAnimation : NetworkBehaviour
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

    [Header("Sprite Game Object")]
    public GameObject GOSprite;
    public GameObject GOSpecialSprite;

    Vector3 moveDirection;
    float moveSpeed = 0f;

    bool idle = true;

    [SyncVar]
    Vector3 PointHand;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }


    [Command]
    void CmdChangePointHand(Vector3 point)
    {
        this.PointHand = point;
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

        //if (!isLocalPlayer)
        //  return;

        if (isLocalPlayer)
        {
            Vector2 mousePosition = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                PointHand = hit.point;
                CmdChangePointHand(PointHand);
            }
        }



            /*Vector2 mousePosition = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            //Vector3 HandPos = PointHand;
            if (isLocalPlayer && Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                PointHand = hit.point;
                CmdChangePointHand(PointHand);
                //HandPos = hit.point;
                //PointHand = hit.point;
            }*/


        if (PointHand.x > this.gameObject.transform.position.x)
        {
            headSpriteRenderer.flipX = false;
            handClothesSpriteRenderer.flipY = false;
        }
        else
        {
            headSpriteRenderer.flipX = true;
            handClothesSpriteRenderer.flipY = true;
        }


        Vector3 playerPosition = this.gameObject.transform.position;
        Vector3 dir = PointHand - playerPosition;
        dir = dir.normalized;

        float angleRad = Mathf.Atan2(dir.z + dir.y, dir.x);
        float angle = angleRad * Mathf.Rad2Deg;
        handGameObject.transform.rotation = Quaternion.Euler(new Vector3(90, 0, angle));

        float scaleY = Mathf.Cos(angleRad * 2.0f) * 0.125f + 1.125f;
        float scaleX = Mathf.Cos((angleRad) * 2.0f + Mathf.PI) * 0.125f + 1.125f;
        handClothesSpriteRenderer.gameObject.transform.localScale = (new Vector3(scaleX, scaleY, 1));

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
        bodyAnimator.CrossFadeInFixedTime(stateName, 0f);
        bodyClothesAnimator.CrossFadeInFixedTime(stateName, 0f);
    }

    void UpAnimatorPlay(string stateName)
    {
        headAnimator.Play(stateName);
        headClothesAnimator.Play(stateName);
        headAnimator.CrossFadeInFixedTime(stateName, 0f);
        headClothesAnimator.CrossFadeInFixedTime(stateName, 0f);
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

    public void ActivateSpecialDist()
    {
        handClothesSpriteRenderer.gameObject.GetComponent<Animator>().Play("Special");
        handClothesSpriteRenderer.gameObject.GetComponent<Animator>().CrossFadeInFixedTime("Special", 0f);
    }

    public void ShowSpecialSprite(bool showSpecialSpriteBool, bool flipX)
    {
        //GOSpecialSprite.GetComponentInChildren<Animator>().CrossFadeInFixedTime("Default", 0f);
        if (flipX)
        {
            GOSpecialSprite.transform.localScale = new Vector3(-1, GOSpecialSprite.transform.localScale.y, GOSpecialSprite.transform.localScale.z);
        } else
        {
            GOSpecialSprite.transform.localScale = new Vector3(1, GOSpecialSprite.transform.localScale.y, GOSpecialSprite.transform.localScale.z);
        }
        GOSpecialSprite.SetActive(showSpecialSpriteBool);
        GOSprite.SetActive(!showSpecialSpriteBool);
        DownAnimatorPlay("Idle");
        UpAnimatorPlay("Idle");
        //GOSpecialSprite.GetComponentInChildren<Animator>().PlayInFixedTime("Default", -1, 0f);
        GOSpecialSprite.GetComponentInChildren<Animator>().Play("Default");
        foreach (SpriteRenderer i in this.GetComponentsInChildren<SpriteRenderer>())
        {
                i.color = Color.white;
        }

    }

    public float GetHandAngle()
    {
        return -handGameObject.transform.rotation.eulerAngles.y;
    }



}
