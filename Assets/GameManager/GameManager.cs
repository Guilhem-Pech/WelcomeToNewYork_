using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(WaveManager))]
[RequireComponent(typeof(PlayerManager))]
public class GameManager : NetworkBehaviour 
{
    [ReadOnly]public WaveManager waveMan;
    [ReadOnly]public PlayerManager playerMan ;

    // Start is called before the first frame update
    
    [ServerCallback] //Reminder: ServerCallback means this function will only be called serverside ( you can create a start function with a [ClientCallback] )
    void Start()
    {
        playerMan = gameObject.GetComponent<PlayerManager>() ;
        playerMan.players.AddRange(GameObject.FindObjectsOfType<BaseChar>());

        foreach (BaseChar entite in playerMan.players)
        {
            print(entite);
        }


        waveMan = gameObject.GetComponent<WaveManager>();
        waveMan.multEnnemiesPerWave = 0;
        Vector3 position =new Vector3(0,1,0);
        playerMan.SpawnAll(position);
        waveMan.DebutVague();
    }


 

    [Server]
    public void FinVague()
    {
        playerMan.RespawnAll();
        playerMan.HealAll();
        waveMan.DebutVague(); 
    }
}
