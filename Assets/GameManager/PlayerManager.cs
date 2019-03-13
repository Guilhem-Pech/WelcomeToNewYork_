using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> joueurMort = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        print("Nombre de Joueur : " + players.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void death(GameObject Entité)
    {
        joueurMort.Add(Entité);
        foreach (SpriteRenderer i in Entité.GetComponentsInChildren<SpriteRenderer>())
        {
            i.color = Color.gray;
        }
        Entité.GetComponent<PlayerController>().enabled = false;
        Entité.GetComponent<PlayerAnimation>().enabled = false;
        Entité.GetComponent<BaseChar>().enabled = false;
        print(joueurMort);
    }

    public void spawnAll(Vector3 position)
    {

    }

    public void respawnAll()
    {
        foreach (GameObject entite in joueurMort)
        {
            foreach (SpriteRenderer i in entite.GetComponentsInChildren<SpriteRenderer>())
            {
                i.color = Color.white;
            }
            entite.GetComponent<PlayerController>().enabled = true;
            entite.GetComponent<PlayerAnimation>().enabled = true;
            entite.GetComponent<BaseChar>().enabled = true;
            entite.GetComponent<BaseChar>().Start();
        }
        joueurMort.Clear();
    }

    public void respawn(GameObject Entite)
    {
        foreach (SpriteRenderer i in Entite.GetComponentsInChildren<SpriteRenderer>())
        {
            i.color = Color.white;
        }
        Entite.GetComponent<PlayerController>().enabled = true;
        Entite.GetComponent<PlayerAnimation>().enabled = true;
        Entite.GetComponent<BaseChar>().enabled = true;
        joueurMort.Remove(Entite);
        Entite.GetComponent<BaseChar>().Start();
    }
}
