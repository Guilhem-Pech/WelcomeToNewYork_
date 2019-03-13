using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int multEnnemiesPerWave = 3;
    public int maxAliveEnnemies = 50;
    [ReadOnly] public int numVague = 0;
    [ReadOnly] public int numEnnemisVague;
    [ReadOnly] public List<GameObject> ennemiVivant;
    public int ennemiRestant;
    private SpawnerManager spawnMan;
    private HordesManager HordeMan;

    // Start is called before the first frame update
    void Start()
    {
        spawnMan = gameObject.AddComponent<SpawnerManager>() as SpawnerManager;
        HordeMan = gameObject.AddComponent<HordesManager>() as HordesManager;
        ennemiVivant = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ennemiRestant == 0 && ennemiVivant.Count == 0)
        {
            print("Fin de la vague numéro : " + numVague );
            this.GetComponentInParent<GameManager>().finVague();        
        }
        if (ennemiRestant > 0 && ennemiVivant.Count < maxAliveEnnemies)
        {
            GameObject ennemi = spawnMan.spawnEnnemiRandom();
            ennemiRestant -= 1;
            ennemiVivant.Add(ennemi);
        }
    }

    public void debutVague()
    {
        numVague += 1;
        print("Numéro de la vague : " + numVague);
        numEnnemisVague = (numVague + this.GetComponentInParent<PlayerManager>().players.Count) * multEnnemiesPerWave;
        ennemiRestant = numEnnemisVague;
        print("Nombre ennemis dans la vague : " + numEnnemisVague);
    }

   
}
