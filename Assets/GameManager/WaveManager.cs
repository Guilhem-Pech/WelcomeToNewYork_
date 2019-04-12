using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(SpawnManager))]
[RequireComponent(typeof(HordesManager))]
public class WaveManager : NetworkBehaviour
{
    public GameObject prefabCaC;
    public GameObject prefabDistance;

    public int multEnnemiesPerWave = 3;
    public int maxAliveEnnemies = 50;
    [ReadOnly] public int numVague = 0;
    [ReadOnly] public int numEnnemisVague;
    [ReadOnly] public List<GameObject> ennemiVivant;
    public int ennemiRestant;
    public SpawnManager spawnMan;
    private HordesManager HordeMan;

    private bool isInWave;
    public float underWaveDelay = 10f;
    private float lastWaveTime;

    // Start is called before the first frame update
    [ServerCallback]
    void Start()
    {
        spawnMan = gameObject.GetComponent<SpawnManager>() as SpawnManager;
        HordeMan = gameObject.GetComponent<HordesManager>() as HordesManager;
        ennemiVivant = new List<GameObject>();
        isInWave = false;
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (ennemiRestant == 0 && ennemiVivant.Count == 0)
        {
            print("Fin de la vague numéro : " + numVague );
            FinVague();
            this.GetComponentInParent<GameManager>().FinVague();        
        }

        if (isInWave)
        {
            lastWaveTime += Time.deltaTime;
            if (lastWaveTime >= underWaveDelay
                && ennemiVivant.Count < maxAliveEnnemies)
            {
                spawnMan.SpawnEnnemiRandom(CreateSpawnList(),3);
                lastWaveTime = 0;
            }
        }
    }

    

    [Server]
    public void DebutVague()
    {
        numVague += 1;
        lastWaveTime = underWaveDelay;
        // print("Numéro de la vague : " + numVague);
        numEnnemisVague = (numVague + this.GetComponentInParent<PlayerManager>().players.Count) * multEnnemiesPerWave;
        ennemiRestant = numEnnemisVague;
        isInWave = true;
       // print("Nombre ennemis dans la vague : " + numEnnemisVague);
    }

    [Server]
    public void FinVague()
    {
        isInWave = false;
    }

    [Server]
    private List<GameObject> CreateSpawnList()
    {
        List<GameObject> spawnList = new List<GameObject>();

        GameObject entity;
        while(ennemiVivant.Count < maxAliveEnnemies)
        {
            entity = Instantiate(((Random.Range(0, 2)) == 0 ? prefabCaC : prefabDistance)
                , transform.position
                , transform.rotation
            ) as GameObject;
            ennemiRestant -= 1;
            ennemiVivant.Add(entity);
        }

        return spawnList;
    }
}

