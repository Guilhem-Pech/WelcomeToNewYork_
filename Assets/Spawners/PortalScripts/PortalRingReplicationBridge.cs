using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PortalRingReplicationBridge : NetworkBehaviour
{
    GameObject anneauInterieur;
    GameObject anneauExterieur;

    private void Awake()
    {
        anneauInterieur = transform.Find("Sprite").Find("AnneauInterieur").gameObject;
        anneauExterieur = transform.Find("Sprite").Find("AnneauExterieur").gameObject;
    }

    public void SetState(bool state)
    {
        if (isServer)
            RpcSetStateOnAllCLients(state);
    }

    [ClientRpc]
    private void RpcSetStateOnAllCLients(bool state)
    {
        anneauInterieur.SetActive(state);
        anneauExterieur.SetActive(state);
    }
}
