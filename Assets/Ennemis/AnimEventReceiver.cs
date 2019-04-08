using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimEventReceiver : MonoBehaviour
{
    private AnimEventReceiverServer anmRcvServ;
    // Start is called before the first frame update
    void Start()
    {
        anmRcvServ = transform.parent.GetComponentInChildren<AnimEventReceiverServer>();
    }

    public void AnimAttackLaunch()
    {
        anmRcvServ.AnimAttackLaunch();
    }

    public void AnimAttackEnd()
    {
        anmRcvServ.AnimAttackEnd();
    }

    public void AnimTeleportLaunch()
    {
        anmRcvServ.AnimTeleportLaunch();
    }

    public void AnimTeleportEnd()
    {
        anmRcvServ.AnimTeleportEnd();
    }
}
