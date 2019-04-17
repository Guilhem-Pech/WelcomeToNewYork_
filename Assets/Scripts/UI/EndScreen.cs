using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : NetworkBehaviour
{
    public GameObject gameOverS;

    public void AfficherGO()
    {
        print("N00B135");
        gameOverS.SetActive(true);
    }

    public void EnleverGO()
    {
        //print("la sorcière te rez");
        gameOverS.SetActive(false);

    }
}
