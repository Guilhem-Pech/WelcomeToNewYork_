using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(SpawnManager))]
[RequireComponent(typeof(HordesManager))]
public class WaveManager : NetworkBehaviour
{
    private DifficultyManager diffMan;

    public float timeWaitFirstSpawnWave = 2f;
    public float timeWaitBetweenSpawnWaves = 10f;

    public bool isInWave;
    private int maxAliveEnnemies;
    private float lastWaveTime;
    [SyncVar (hook = nameof(OnChangeNumWave))] public int numVague = 0;
    [SyncVar] public int numEnnemisVague;
    public List<GameObject> ennemiVivant;
    [SyncVar (hook =nameof(OnChangeEnnemiVivant))] public int nbEnnemisVivant;
    [SyncVar(hook = nameof(OnChangeEnnemiRestant))] public int ennemiRestant;
    public SpawnManager spawnMan;
    private HordesManager HordeMan;
    public WaveText waveText;
    public MonstersRemaining monstersRemaining;

    public void OnChangeNumWave(int wave)
    {
        waveText.SetWaveStartedText((uint)wave);
        numVague = wave;
    }

    public void OnChangeEnnemiRestant(int num)
    {
        
        monstersRemaining.SetNumber((uint)(num + nbEnnemisVivant));
        ennemiRestant = num;
    }


    public void OnChangeEnnemiVivant(int num)
    {
        monstersRemaining.SetNumber((uint) (num + ennemiRestant));
        nbEnnemisVivant = num;
    }

    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
        diffMan = gameObject.GetComponent<DifficultyManager>() as DifficultyManager;
        spawnMan = gameObject.GetComponent<SpawnManager>() as SpawnManager;
        HordeMan = gameObject.GetComponent<HordesManager>() as HordesManager;
        ennemiVivant = new List<GameObject>();

        //Debug.Log("Init. Manager de Vague!");
        isInWave = false;
        if(waveText == null)
            waveText = GameObject.Find("WaveText").GetComponent<WaveText>();
        if (monstersRemaining == null)
            monstersRemaining = GameObject.Find("Monsters").GetComponent<MonstersRemaining>();
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (isInWave && ennemiRestant == 0 && ennemiVivant.Count == 0)
        {
            //print("Fin de la vague numéro : " + numVague );
            FinVague();
            this.GetComponentInParent<GameManager>().FinVague();
        }

        if (isInWave)
        {
            lastWaveTime += Time.deltaTime;
            if ((lastWaveTime >= timeWaitBetweenSpawnWaves
                && ennemiVivant.Count < maxAliveEnnemies)
                && ennemiRestant > 0)
            {
                //Debug.Log("Lancement d'un Spawn");
                spawnMan.SpawnEnnemiRandom(CreateSpawnList(),3);
                lastWaveTime = 0;
            }
        }
        nbEnnemisVivant = ennemiVivant.Count;
    }



    [Server]
    public void DebutVague()
    {
        numVague += 1;
        lastWaveTime = timeWaitBetweenSpawnWaves - timeWaitFirstSpawnWave;
        // print("Numéro de la vague : " + numVague);
        //Debug.Log("Début de Vague!");
        isInWave = true;
        maxAliveEnnemies = diffMan.getMaxEnnemyAliveForWave(numVague);
        numEnnemisVague = diffMan.getNumberOfEnnemiesForWave(numVague);
        ennemiRestant = numEnnemisVague;
        spawnMan.OnCurrentSpawnersChange();
        // print("Nombre ennemis dans la vague : " + numEnnemisVague);
    }

    [Server]
    public void FinVague()
    {
        RpcFinVague(numVague);
        isInWave = false;
    }

    [ClientRpc]
    public void RpcFinVague(int num)
    {
        waveText.SetWaveEndedText((uint)num);
    }


    [Server]
    private List<GameObject> CreateSpawnList()
    {
        List<GameObject> spawnList = new List<GameObject>();

        GameObject entity;
        Vector3 bufferSpawnPos = new Vector3(-1000, -1000, -1000);
       
        while(ennemiVivant.Count < maxAliveEnnemies && ennemiRestant > 0)
        {
            GameObject ennemyPrefab = diffMan.getRandomEnnemyPrefab();
            entity = Instantiate(ennemyPrefab
                , transform.position
                , transform.rotation
            ) as GameObject;
            entity.transform.position = bufferSpawnPos;
            entity.SetActive(false);
            ennemiRestant -= 1;
            ennemiVivant.Add(entity);

            spawnList.Add(entity);
        }

        return spawnList;
    }
}
