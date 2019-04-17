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
        print("Tu es mort");
        
        deathScreen.SetActive(true);
    }

    public void EnleverLabelMort()
    {
        print("la sorcière te rez");
        deathScreen.SetActive(false);

    }
}
