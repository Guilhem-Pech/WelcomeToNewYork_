﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : NetworkBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject mainMenu;
    public GameObject optionMenu;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
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
        gameIsPaused = false;
        optionMenu.SetActive(false);

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
       // Debug.Log("QUIT!");
        Application.Quit();
    }

    public void MenuGame()
    {
        Time.timeScale = 1;

        print(FindObjectOfType<WTNYNetworkLobbyManager>().client);

        FindObjectOfType<WTNYNetworkLobbyManager>().StopHost();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

  
}
