using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LoadSceneManager : NetworkBehaviour
{
    private bool loadScene = false;

    public void LauchLoadingScreen()
    {
        if(loadScene == false)
        {
            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            if (isServer)
                RpcLauchLoadingScreenOnAllCLients();

            /*// ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());*/
        }
    }

    [ClientRpc]
    public void RpcLauchLoadingScreenOnAllCLients()
    {
        Debug.Log(isClientOnly);
        gameObject.transform.parent.parent.Find("LobbyMenuJoin").gameObject.SetActive(false);
        gameObject.transform.parent.parent.Find("VideoNY").gameObject.SetActive(false);
        gameObject.transform.parent.parent.Find("LoadingScreen").gameObject.SetActive(true);
        gameObject.transform.Find("Text").GetComponent<TextLoad>().StartPrinting();
    }
}
