
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATHSettings : MonoBehaviour {


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

        int ts = FindObjectOfType<WaveManager>().ennemiRestant;
       

        Text nbRestants = GameObject.Find("EnnemisRestants").GetComponent<Text>();
        nbRestants.text = ts.ToString();


    }
}
