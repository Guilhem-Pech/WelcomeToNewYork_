using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEffectTurn : MonoBehaviour
{

    GameObject thisGO;
    float angle;
    public float speed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        thisGO = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.thisGO.transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
