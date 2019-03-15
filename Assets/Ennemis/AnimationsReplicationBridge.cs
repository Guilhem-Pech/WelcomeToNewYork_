using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AnimationsReplicationBridge : NetworkBehaviour
{
    Animator animationsController;
   
    private void Awake()
    {
        animationsController = transform.Find("Sprite").GetComponent<Animator>();
    }
    
    public void playAnimation(string animationID)
    {
            RpcPlayAnimationOnAllCLients(animationID);
    }

    [ClientRpc]
    private void RpcPlayAnimationOnAllCLients(string animationID)
    {
        animationsController.Play(animationID);
    }
}
