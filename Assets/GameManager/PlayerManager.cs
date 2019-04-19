
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerManager : NetworkBehaviour
{
    public List<GameObject> joueurMort = new List<GameObject>();
    public List<BaseChar> players = new List<BaseChar>();


    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
      //  print("Nombre de Joueur : " + players.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Server]
    public void Death(GameObject entity)
    {
        if (!joueurMort.Contains(entity.gameObject))
        {
            joueurMort.Add(entity.gameObject);
            RpcDeath(entity.gameObject);
        }
        
       // Debug.Log("PLQYErS  ==========     " + players.Count());
       // Debug.Log("MORTS  ==========     " + joueurMort.Count());



        if (players.Count() == joueurMort.Count())
        {
            foreach (GameObject entite in joueurMort)
            {
                RpcGameOver(entite);
            }
        }
    }

   [ClientRpc]
   public void RpcDeath(GameObject entity)
    {
        foreach (MeleeAttack aMeleeAttack in entity.GetComponentsInChildren<MeleeAttack>())
            Destroy(aMeleeAttack.gameObject);


        entity.GetComponent<PlayerAnimation>().DisplayHands(true);
        entity.GetComponent<PlayerAnimation>().ShowSpecialSprite(false, false);
        if(entity.GetComponent<MeleeChar>())
        {
            entity.GetComponent<MeleeChar>().isAttacking = false;
        }

        foreach (SpriteRenderer i in entity.GetComponentsInChildren<SpriteRenderer>())
        {
            i.color = Color.gray;
        }
        entity.GetComponent<PlayerController>().enabled = false;
        entity.GetComponent<PlayerAnimation>().enabled = false;
        entity.GetComponent<BaseEntity>().enabled = false;

       /* GameObject uiInGame = GameObject.Find("UI");
        uiInGame.GetComponent<DeathScreen>().AfficherLabelMort();*/
    }

    public void SpawnAll(Vector3 position)
    {

    }

    [Server]
    public void RespawnAll()
    {
        foreach (GameObject entite in joueurMort)
        {
            Respawn(entite);
        }
        joueurMort.Clear();

       // Debug.Log("MANGE TES GRANDS MORTS         " + joueurMort.Count());
    }

    [Server]
    public void HealAll()
    {
        foreach (BaseChar entite in players)
        {
            print(entite.name);
            print(entite.GetComponent<BaseChar>().GetMaxHealth() + " - " + entite.GetComponent<BaseChar>().currentHealth);

            entite.GetComponent<BaseChar>().AddHealth(entite.GetComponent<BaseChar>().GetMaxHealth() - entite.GetComponent<BaseChar>().currentHealth);
        }
    }


    [Server]
    public void Respawn(GameObject entity)
    {
        joueurMort.Remove(entity);
        RpcRespawn(entity);
        entity.GetComponent<BaseChar>().Start();
    }

    [ClientRpc]
    public void RpcRespawn(GameObject entity)
    {
        foreach (SpriteRenderer i in entity.GetComponentsInChildren<SpriteRenderer>())
        {
            i.color = Color.white;
        }
        entity.GetComponent<PlayerController>().enabled = true;
        entity.GetComponent<PlayerAnimation>().enabled = true;
        entity.GetComponent<BaseChar>().enabled = true;
        entity.GetComponent<BaseChar>().Start();

        GameObject UI = GameObject.Find("UI");
        UI.GetComponent<DeathScreen>().EnleverLabelMort();
    }


    [ClientRpc]
    public void RpcGameOver(GameObject entity)
    {
        //Affiche un panel avec les stats de la game puis renvoie tout le monde au menu
       // Debug.Log("Vous êtes nuls ptdr");
        GameObject UI = GameObject.Find("UI");
        UI.GetComponent<EndScreen>().AfficherGO();
    }
}
