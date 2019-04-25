﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class WTNYNetworkLobbyManager : NetworkLobbyManager
{
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

    public override void OnLobbyServerPlayersReady()
    {
        Debug.Log(GameObject.Find("CanvasLS").transform.Find("LoadingScreen"));
        GameObject.Find("CanvasLS").transform.Find("LoadingScreen").GetComponentInChildren<LoadSceneManager>().LauchLoadingScreen();
        base.OnLobbyServerPlayersReady();
    }
}
