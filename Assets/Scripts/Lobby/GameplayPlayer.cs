using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameplayPlayer : NetworkBehaviour
{
    public GameObject realGameplay;

    [ServerCallback]
    private void Start()
    {
        NetworkConnection conn = this.connectionToClient;
        GameObject newPlayer = Instantiate<GameObject>(realGameplay);
        newPlayer.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer);
    }
}
