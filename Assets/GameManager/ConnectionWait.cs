using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ConnectionWait : NetworkBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (NetworkServer.connections.Count == GameObject.FindGameObjectsWithTag("Player").Length)
            {
                RpcTimeStartOnAllCLients();
            }
        }
    }

    [ClientRpc]
    private void RpcTimeStartOnAllCLients()
    {
        Time.timeScale = 1f;
    }
}
