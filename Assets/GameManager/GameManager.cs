using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [ReadOnly]public WaveManager waveMan;
    [ReadOnly]public PlayerManager playerMan ;

    // Start is called before the first frame update

    void Start()
    {
        playerMan = gameObject.AddComponent<PlayerManager>() as PlayerManager ;
        waveMan = gameObject.AddComponent<WaveManager>() as WaveManager;
        Vector3 position =new Vector3(0,1,0);
        playerMan.spawnAll(position);
        waveMan.debutVague();
    }
 


    public void finVague()
    {
        playerMan.respawnAll();
        waveMan.debutVague(); 
    }
}
