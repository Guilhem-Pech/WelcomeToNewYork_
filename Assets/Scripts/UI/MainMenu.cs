using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MainMenu : MonoBehaviour
{
    private NetworkLobbyManager networkLobbyManager;
    // Start is called before the first frame update
    private void Start()
    {
        networkLobbyManager= GameObject.FindObjectOfType<NetworkLobbyManager>();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreateServer()
    {
        
        networkLobbyManager.StartHost();
    }
   
    public void QuitGame()
    {
       // Debug.Log("QUIT!");
        Application.Quit();
    }
}
