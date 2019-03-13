
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATHSettings : MonoBehaviour { 

    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        GameObject PersonnageGameObject = GameObject.FindGameObjectWithTag("Player");
        BaseChar Personnage = PersonnageGameObject.GetComponent<BaseChar>();

        HealthATH(Personnage);
        StaminaATH(Personnage);
        EnnemyATH();

        //JojoChar truc = GetComponent<JojoChar>();

        //Debug.Log(health);
    }

    void HealthATH(BaseChar truc)
    {
        
        float health = truc.setHeal();
        float maxHealth = truc.getMaxHealth();

        Scrollbar BarrePV = GameObject.Find("BarrePV").GetComponent<Scrollbar>();

        BarrePV.size = health / maxHealth;
    }

    void StaminaATH(BaseChar truc)
    {
        float stamina = truc.getStamina();
        float maxStamina = truc.getMaxStamina();

        Scrollbar BarreMana = GameObject.Find("BarreMana").GetComponent<Scrollbar>();

        BarreMana.size = stamina / maxStamina;
    }

    void EnnemyATH() {
       
        var ts = GameObject.FindGameObjectsWithTag("ennemy");
        //Debug.Log(ts);
        int count = 0;
        foreach(GameObject t in ts)
        {
            //Debug.Log(t.name);
            if (t.name == "Ennemi(Clone)")
            {
                count++;
            }
        }

        Text nbRestants = GameObject.Find("EnnemisRestants").GetComponent<Text>();
        nbRestants.text = count.ToString();


    }
}
