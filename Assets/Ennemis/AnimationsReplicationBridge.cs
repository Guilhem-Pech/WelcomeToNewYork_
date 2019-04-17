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

    public void SetBoolParam(string paramName, bool value)
    {
        if (isServer)
            RpcSetBoolParamOnAllCLients(paramName, value);
    }

    [ClientRpc]
    private void RpcSetBoolParamOnAllCLients(string paramName, bool value)
    {
        animationsController.SetBool(paramName, value);
    }

    public void playAnimation(string animationID)
    {
        if(isServer)
            RpcPlayAnimationOnAllCLients(animationID);
    }

    [ClientRpc]
    private void RpcPlayAnimationOnAllCLients(string animationID)
    {
        animationsController.Play(animationID);
    }
}
