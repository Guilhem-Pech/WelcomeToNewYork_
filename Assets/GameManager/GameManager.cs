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

    public float timeWaitFirstWave = 10f;
    public float timeWaitBetweenWaves = 10f;

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
        Vector3 position =new Vector3(0,1,0);
        playerMan.SpawnAll(position);
        StartCoroutine("TimerStart", timeWaitFirstWave);
    }

    [Server]
    public void FinVague()
    {
        playerMan.RespawnAll();
        playerMan.HealAll();
        StartCoroutine("TimerStart", timeWaitBetweenWaves);
    }

    IEnumerator TimerStart(float time)
    {
        yield return new WaitForSeconds(time);
        playerMan.RespawnAll();
        playerMan.HealAll();
        waveMan.DebutVague();
        yield return null;
    }
}
