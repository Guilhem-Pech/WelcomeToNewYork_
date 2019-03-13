﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WaveManager : NetworkBehaviour
{
    public int multEnnemiesPerWave = 1;
    public int maxAliveEnnemies = 5;
    [ReadOnly] public int numVague = 0;
    [ReadOnly] public int numEnnemisVague;
    [ReadOnly] public List<GameObject> ennemiVivant;
    public int ennemiRestant;
    private SpawnerManager spawnMan;
    private HordesManager HordeMan;

    // Start is called before the first frame update
    [ServerCallback]
    void Start()
    {
        spawnMan = gameObject.AddComponent<SpawnerManager>() as SpawnerManager;
        HordeMan = gameObject.AddComponent<HordesManager>() as HordesManager;
        ennemiVivant = new List<GameObject>();
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (ennemiRestant == 0 && ennemiVivant.Count == 0)
        {
            print("Fin de la vague numéro : " + numVague );
            this.GetComponentInParent<GameManager>().FinVague();        
        }
        if (ennemiRestant > 0 && ennemiVivant.Count < maxAliveEnnemies)
        {
            GameObject ennemi = spawnMan.SpawnEnnemiRandom();
            ennemiRestant -= 1;
            ennemiVivant.Add(ennemi);
        }
    }

    [Server]
    public void DebutVague()
    {
        numVague += 1;
        print("Numéro de la vague : " + numVague);
        numEnnemisVague = (numVague + this.GetComponentInParent<PlayerManager>().players.Count) * multEnnemiesPerWave;
        ennemiRestant = numEnnemisVague;
        print("Nombre ennemis dans la vague : " + numEnnemisVague);
    }

   
}
