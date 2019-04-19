using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : NetworkBehaviour
{
    public GameObject gameOverS;

    public void AfficherGO()
    {
        gameOverS.SetActive(true);
    }

    public void EnleverGO()
    {
        gameOverS.SetActive(false);

    }
}
