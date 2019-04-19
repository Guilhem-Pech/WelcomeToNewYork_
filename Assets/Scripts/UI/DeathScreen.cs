using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeathScreen : NetworkBehaviour
{
    public GameObject deathScreen;

    void Update() { }


    public void AfficherLabelMort()
    {
        
        deathScreen.SetActive(true);
    }

    public void EnleverLabelMort()
    {
        deathScreen.SetActive(false);

    }
}
