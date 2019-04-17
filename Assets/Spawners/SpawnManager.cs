using System.Collections;
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

        OnCurrentSpawnersChange();
    }

    /* Var change methods */
    [Server]
    public void OnCurrentSpawnersChange()
    {
        //Debug.Log("Changement dans les spawners : ");
        FetchSpawnersList();
        //Debug.Log("     Nb trouvés : " + currentSpawners.Count);
        UpdateAndCleanList();
        //Debug.Log("     Nb trouvés après nettoyage : " + currentSpawnersList.Count);
        EvaluateSpawners();
        //Debug.Log("     Nb évalués : " + evaluatedSpawnersList.Count);
    }

    [Server]
    private void FetchSpawnersList()
    {
        currentSpawners = new Dictionary<int, GameObject>();
        foreach (BaseChar player in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().playerMan.players)
        {
            if (!player.enabled)
                continue;
            foreach (KeyValuePair<int, GameObject> it in player.gameObject.transform.GetComponentInChildren<SpawnTargetController>().GetCurrentSpawners())
            {
                if (currentSpawners.ContainsKey(it.Key))
                    continue;

                currentSpawners.Add(it.Key, it.Value);
            }
           
        }
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
        //Debug.Log("Affichage de l'évaluation :");
        //On évalue tout les spawners
        foreach (GameObject spawner in currentSpawnersList)
        {
            float scoreSum = 0;
            foreach (BaseChar player in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().playerMan.players)
            {
                if (!player.enabled)
                    continue;
                SpawnTargetController trgtCtrl = player.gameObject.transform.GetComponentInChildren<SpawnTargetController>() as SpawnTargetController;
                scoreSum += trgtCtrl.EvaluateSpawner(spawner);
            }
            spawnersDictionnaryBuffer.Add(spawner, scoreSum);
        }
        //Debug.Log("Fin Affichage résultat");

        //Debug.Log("Affichage du résultat de l'évaluation :");
        //On trie les spawners
        foreach (KeyValuePair<GameObject, float> item in spawnersDictionnaryBuffer.OrderByDescending(key => key.Value))
        {
            //Debug.Log("     Spawner <" + item.Key.name + "> :" + item.Value);
            evaluatedSpawnersList.Add(item.Key);
        }
        //Debug.Log("Fin Affichage résultat");
    }

    [Server]
    public int HowMuchSpawnersAvaible()
    {
        return evaluatedSpawnersList.Count;
    }

    [Server]
    public List<GameObject> GetTopSpawners(int n)
    {
        //Debug.Log("Nb spawners évalués : " + evaluatedSpawnersList.Count);
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

        //Debug.Log("Répartition de la liste de Spawn");
        //Debug.Log("     Ennemis restants à répartir : " + prefabsList.Count);
        while (prefabsList.Count > 0)
        {
            if (prefabsList.Count >= spawners[it].GetComponent<SpawnerController>().spawnCapacity)
                nbToCut = spawners[it].GetComponent<SpawnerController>().spawnCapacity;
            else
                nbToCut = prefabsList.Count;

            //Debug.Log("     Spawner <" + spawners[it] .name+ "> : " + nbToCut + " ennemis ajoutés.");
            spawners[it].GetComponent<SpawnerController>().AddToSpawnList(prefabsList.GetRange(0, nbToCut));
            prefabsList.RemoveRange(0, nbToCut);
            //Debug.Log("     Ennemis restants à répartir : " + prefabsList.Count);

            if (it == (spawners.Count - 1))
                it = 0;
            else
                it++;
        }
        //Debug.Log("Fin de la répartition de la liste de Spawn");
    }

}
