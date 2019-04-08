using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class AnimEventReceiverServer : NetworkBehaviour
{
    private AttackSystem atkSys;
    // Start is called before the first frame update
    void Start()
    {
        atkSys = transform.GetComponentInChildren<AttackSystem>();
    }

    public void AnimAttackLaunch()
    {
        if (isServer)
            atkSys.AnimAttackLaunch();
    }

    public void AnimAttackEnd()
    {
        if (isServer)
            atkSys.AnimAttackEnd();
    }

    public void AnimTeleportLaunch()
    {
        if (isServer)
            atkSys.AnimTeleportLaunch();
    }

    public void AnimTeleportEnd()
    {
        if (isServer)
            atkSys.AnimTeleportEnd();
    }
}
