
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATHSettings : MonoBehaviour {

    WaveManager waveManager;
    GameObject personnageGameObject;
    BaseChar personnage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (personnage == null)
        {
            if (personnageGameObject == null)
            {
                personnageGameObject = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                personnage = personnageGameObject.GetComponent<BaseChar>();
            }
            return;
        }

        EnnemyATH();

        //JojoChar truc = GetComponent<JojoChar>();

        //Debug.Log(health);
    }

    

    void EnnemyATH() {

        if (waveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
            return;
        }
            

        int ts = waveManager.ennemiRestant + waveManager.ennemiVivant.Count;
       

        Text nbRestants = GameObject.Find("EnnemisRestants").GetComponent<Text>();
        nbRestants.text = ts.ToString();


    }
}
