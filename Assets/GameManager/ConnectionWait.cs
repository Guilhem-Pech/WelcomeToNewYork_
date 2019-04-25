using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ConnectionWait : NetworkBehaviour
{
    private bool loadDone;
    private void Awake()
    {
        loadDone = false;
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (NetworkServer.connections.Count == GameObject.FindGameObjectsWithTag("Player").Length && !loadDone)
            {
                loadDone = true;
                RpcTimeStartOnAllCLients();
            }
        }
    }

    [ClientRpc]
    private void RpcTimeStartOnAllCLients()
    {
        Time.timeScale = 1f;
        SoundManager.instance.PlayAmbiant();
        Destroy(GameObject.Find("CanvasLS").gameObject);
    }
}
