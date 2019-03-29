using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeathScreen : NetworkBehaviour
{
    public GameObject DSTemp;

    void Update() { }


    public void AfficherLabelMort()
    {
        print("Tu es mort");
        DSTemp.SetActive(true);
    }

    public void EnleverLabelMort()
    {
        print("la sorcière te rez");
        DSTemp.SetActive(false);

    }
}
