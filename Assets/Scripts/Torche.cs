using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Torche : MonoBehaviour
{
    private GameObject hand; // On récupère la main
    
    private float handOrientation = 0; //orientation en z de la main
    

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand");
    }

    // Update is called once per frame
    void Update()
    {
        handOrientation = GetComponentInParent<PlayerAnimation>().GetHandAngle(); //récuperation de l'orientation
        handOrientation = -handOrientation;

        //On calcul l'écart entre l'orientation de la main et celle de la flashlight en prenant en compte le décalage
        float difference = handOrientation - transform.rotation.eulerAngles.y + 90;

        transform.RotateAround(this.transform.parent.transform.position, Vector3.up, difference    ); //on applique la rotation en y
    }
}
