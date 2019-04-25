using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class WTNYNetworkLobbyManager : NetworkLobbyManager
{
    private static int count = 0;
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        gamePlayer.GetComponent<GameplayPlayer>().realGameplay = lobbyPlayer.GetComponent<LobbyPlayer>().choosenChar.gameObject; 
        return true;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerManager.GetPlayerMan().players.Remove(conn.playerController.gameObject.GetComponent<BaseChar>());
        base.OnServerDisconnect(conn);
    }
}
