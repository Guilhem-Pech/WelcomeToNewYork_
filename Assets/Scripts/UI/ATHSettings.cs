
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

        HealthATH();
        StaminaATH();
        EnnemyATH();

        //JojoChar truc = GetComponent<JojoChar>();

        //Debug.Log(health);
    }

    void HealthATH()
    {
       
        float health = personnage.GetHealth();
        float maxHealth = personnage.GetMaxHealth();

        //Scrollbar BarrePV = GameObject.Find("BarrePV").GetComponent<Scrollbar>();

        //BarrePV.size = health / maxHealth;
    }

    void StaminaATH()
    {
        float stamina = personnage.getStamina();
        float maxStamina = personnage.getMaxStamina();

        //Scrollbar BarreMana = GameObject.Find("BarreMana").GetComponent<Scrollbar>();

        //BarreMana.size = stamina / maxStamina;
    }

    void EnnemyATH() {

        int ts = FindObjectOfType<WaveManager>().ennemiRestant;
       

        Text nbRestants = GameObject.Find("EnnemisRestants").GetComponent<Text>();
        nbRestants.text = ts.ToString();


    }
}
