using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 3f;
    public bool mooveEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDirection.magnitude > 1)
            moveDirection = moveDirection.normalized; 
    }


    // Update is called once per frame
    void Update()
    {
        if (mooveEnable)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    public Vector3 getMoveDirection()
    {
        return moveDirection;
    }
}
