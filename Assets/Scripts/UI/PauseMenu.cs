using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PauseMenu : NetworkBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    private NetworkManager manager;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void MenuGame()
    {
        Time.timeScale = 1;

        manager = GameObject.FindObjectOfType<NetworkManager>();
        if (isServer)
        {
            Debug.Log(manager);
            manager.StopHost();
            Debug.Log("SERVEUR STOP");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

  
}
