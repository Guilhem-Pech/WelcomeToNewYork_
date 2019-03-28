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
}
