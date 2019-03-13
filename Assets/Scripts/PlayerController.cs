using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    
    Rigidbody rb;
    [SyncVar]
    Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 3f;
    public bool mooveEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isLocalPlayer)
            transform.Find("Main Camera").gameObject.SetActive(true);
    }

    void FixedUpdate()
    {

        if (!isLocalPlayer)
            return;

        CmdSetMovedir(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

    }

    [Command]
    void CmdSetMovedir(Vector3 imput)
    {
        moveDirection = imput;
        if (moveDirection.magnitude > 1)
            moveDirection = moveDirection.normalized;
    }

    // Update is called once per frame
    void Update()
    {
      rb.velocity = moveDirection* moveSpeed;   
    }

    public Vector3 getMoveDirection()
    {
        return rb.velocity.normalized;
    }
}
