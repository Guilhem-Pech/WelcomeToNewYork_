using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
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
    }
}
