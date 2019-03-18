using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System;

public class LobbyHUD : MonoBehaviour
{
    private NetworkLobbyManager manager;

    [SerializeField]
    public List<NetworkIdentity> GameplayPlayers;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("NetworkLobby").GetComponent<NetworkLobbyManager>();

        var adressText = GameObject.Find("TextIP").GetComponent<Text>();
        adressText.text = GetLocalIPAddress();
      //  Debug.Log(GetLocalIPAddress());
    }


    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public void StopServerHost()
    {
        
        manager.StopHost();

    }

    public void StopClient()
    {

        manager.StopHost();
        
    }

    void SetReady()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {

      
       
    }
}
