﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class SpawnManager : NetworkBehaviour
{

    private Dictionary<int, GameObject> currentSpawners;
    private List<GameObject> currentSpawnersList;
    private List<GameObject> evaluatedSpawnersList;

    [ServerCallback]
    void Start()
    {
        evaluatedSpawnersList = new List<GameObject>();
        currentSpawnersList = new List<GameObject>();
        currentSpawners = new Dictionary<int, GameObject>();
    }

    /* Getters */
    public bool IsSpawnerSuscribed(GameObject entity)
    {
        return currentSpawners.ContainsKey(entity.GetInstanceID());
    }

    /* Pop. Managers */
    [Server]
    public void AddSpawner(GameObject spawnerToAdd)
    {

        OnCurrentSpawnersChange();
    }

    [Server]
    public void RemoveSpawner(GameObject spawnerToRemove)
    {

        OnCurrentSpawnersChange();
    }

    /* Var change methods */
    [Server]
    private void OnCurrentSpawnersChange()
    {
        UpdateAndCleanList();
        EvaluateSpawners();
    }

    [Server]
    private void UpdateAndCleanList()
    {
        currentSpawnersList = new List<GameObject>();
        List<int> toDeleteNeighboursList = new List<int>();

        foreach (KeyValuePair<int, GameObject> neighbour in currentSpawners)
        {
            if (neighbour.Value == null)
                toDeleteNeighboursList.Add(neighbour.Key);
            else
                currentSpawnersList.Add(neighbour.Value);
        }

        foreach (int key in toDeleteNeighboursList)
        {
            currentSpawners.Remove(key);
        }
    }

    /* Evaluation des Spawners */
    [Server]
    private void EvaluateSpawners()
    {
        evaluatedSpawnersList = new List<GameObject>();

        Dictionary<GameObject, float> spawnersDictionnaryBuffer = new Dictionary<GameObject, float>();
        //On évalue tout les spawners
        foreach (GameObject spawner in currentSpawnersList)
        {
            float scoreSum = 0;
            foreach (BaseChar player in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().playerMan.players)
            {
                if (!player.enabled)
                    continue;
                scoreSum += player.gameObject.GetComponent<SpawnTargetController>().EvaluateSpawner(spawner);
            }
            spawnersDictionnaryBuffer.Add(spawner, scoreSum);
        }

        //On trie les spawners
        foreach (KeyValuePair<GameObject, float> item in spawnersDictionnaryBuffer.OrderByDescending(key => key.Value))
        {
            evaluatedSpawnersList.Add(item.Key);
        }
    }

    [Server]
    public int HowMuchSpawnersAvaible()
    {
        return evaluatedSpawnersList.Count;
    }

    [Server]
    public List<GameObject> GetTopSpawners(int n)
    {
        if (n > evaluatedSpawnersList.Count)
            n = evaluatedSpawnersList.Count;
        return evaluatedSpawnersList.GetRange(0,n);
    }

    /* Répartition du spawn d'un liste d'ennemis */
    [Server]
    public void SpawnEnnemiRandom(List<GameObject> prefabsList, int nbSpawners)
    {
        List<GameObject> spawners = GetTopSpawners(nbSpawners);
        int it = 0;

        int nbToCut;
        while (prefabsList.Count != 0)
        {
            if (prefabsList.Count >= spawners[it].GetComponent<SpawnerController>().spawnCapacity)
                nbToCut = spawners[it].GetComponent<SpawnerController>().spawnCapacity;
            else
                nbToCut = prefabsList.Count;

            spawners[it].GetComponent<SpawnerController>().AddToSpawnList(prefabsList.GetRange(0, nbToCut-1));
            prefabsList.RemoveRange(0, nbToCut - 1);

            if (it == (spawners.Count - 1))
                it = 0;
            else
                it++;
        }
    }

}
