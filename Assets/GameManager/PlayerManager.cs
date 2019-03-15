using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public List<GameObject> joueurMort = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();


    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
      //  print("Nombre de Joueur : " + players.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Server]
    public void Death(GameObject entity)
    {
        joueurMort.Add(entity.gameObject);
        RpcDeath(entity.gameObject);
        print(joueurMort);
    }

    [ClientRpc]
   public void RpcDeath(GameObject entity)
    {
        foreach (SpriteRenderer i in entity.GetComponentsInChildren<SpriteRenderer>())
        {
            i.color = Color.gray;
        }
        entity.GetComponent<PlayerController>().enabled = false;
        entity.GetComponent<PlayerAnimation>().enabled = false;
        entity.GetComponent<BaseChar>().enabled = false;
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
    }
}
