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
    // Start is called before the first frame update
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
    public void AddSpawner(GameObject spawnerToAdd)
    {

        OnCurrentSpawnersChange();
    }

    public void RemoveSpawner(GameObject spawnerToRemove)
    {

        OnCurrentSpawnersChange();
    }

    /* Var change methods */
    private void OnCurrentSpawnersChange()
    {
        UpdateAndCleanList();
        EvaluateSpawners();
    }

    private void UpdateAndCleanList()
    {
        currentSpawnersList = new List<GameObject>();
        List<int> toDeleteNeighboursList = new List<int>();

        foreach (KeyValuePair<int, GameObject> neighbour in currentSpawners)
        {
            if (neighbour.Value == null)
            {
                toDeleteNeighboursList.Add(neighbour.Key);
            }
            else
            {
                currentSpawnersList.Add(neighbour.Value);
            }
        }

        foreach (int key in toDeleteNeighboursList)
        {
            currentSpawners.Remove(key);
        }
    }

    /* Evaluation des Spawners */
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

    public int HowMuchSpawnersAvaible()
    {
        return evaluatedSpawnersList.Count;
    }

    public List<GameObject> GetTopSpawners(int n)
    {
        return evaluatedSpawnersList.GetRange(0,n);
    }
}
