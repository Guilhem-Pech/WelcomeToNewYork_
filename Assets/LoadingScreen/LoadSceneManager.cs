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
    private void RpcLauchLoadingScreenOnAllCLients()
    {
        gameObject.transform.parent.Find("LobbyMenuJoin").gameObject.SetActive(false);
        gameObject.transform.parent.Find("VideoNY").gameObject.SetActive(false);
        gameObject.transform.parent.Find("LoadingScreen").gameObject.SetActive(true);
    }

    /*
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(3);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = Application.LoadLevelAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }*/

     
}
