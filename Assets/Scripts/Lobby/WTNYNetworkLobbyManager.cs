using System.Collections;
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

    public override void OnLobbyServerPlayersReady()
    {
        

        base.OnLobbyServerPlayersReady();
    }

    public override void ServerChangeScene(string sceneName)
    {
        Debug.Log("Changing Scene !");
        GameObject loadScreen = GameObject.Find("Canvas").transform.Find("LoadingScreen").Find("Panel").gameObject;
        if (loadScreen != null)
        {
            Debug.Log("Sending Loading screen show up!");
            loadScreen.GetComponent<LoadSceneManager>().LauchLoadingScreen();
        }
        numberFinishedLoading = 0;
        base.ServerChangeScene(sceneName);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("Scene Changed !");
        numberFinishedLoading++;

        base.OnClientSceneChanged(conn);
    }

    public override void OnStartServer()
    {
        numberToWaitFor = 0;
        base.OnStartServer();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        numberToWaitFor++;

        Debug.Log("numberToWaitFor = " + numberToWaitFor);
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        numberToWaitFor--;
        base.OnClientDisconnect(conn);
    }
}
