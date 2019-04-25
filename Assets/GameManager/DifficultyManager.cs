using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public int baseNumberOfEnnemiesPerWave = 25;
    public float growthNumberOfEnnemiesPerWave = 0.25f;
    public int baseMaxEnnemiesAlive = 10;
    public float growthMaxEnnemiesAlive = 0.1f;
    public float[] waveLengthModifiers = {1,2,2.75f,3.5f,4f,4.5f,5f,5.5f};

    public float tankEnnemyProbThreshhold = 0.15f;
    public float rangedEnnemyProbThreshhold = 0.4f;
    public float normalEnnemyProbThreshhold = 1f;

    public GameObject prefabCaC;
    public GameObject prefabDistance;
    public GameObject prefabTank;

    public int getMaxEnnemyAliveForWave(int wave)
    {
        int nbJoueurs = gameObject.GetComponent<PlayerManager>().players.Count;
        float waveLengthModifier = waveLengthModifiers[nbJoueurs - 1];
        float growthModifier = 1 +(growthMaxEnnemiesAlive * (wave - 1));

        return (int) Mathf.Round((baseMaxEnnemiesAlive * waveLengthModifier) * growthModifier);
    }

    public int getNumberOfEnnemiesForWave(int wave)
    {
        int nbJoueurs = gameObject.GetComponent<PlayerManager>().players.Count;
        float waveLengthModifier = waveLengthModifiers[nbJoueurs - 1];
        float growthModifier = 1 + (growthNumberOfEnnemiesPerWave * (wave - 1));

        return (int)Mathf.Round((baseNumberOfEnnemiesPerWave * waveLengthModifier) * growthModifier);
    }

    public GameObject getRandomEnnemyPrefab()
    {
        float dé = Random.Range(0.0f, 1.0f);

        if (dé < tankEnnemyProbThreshhold)
            return prefabTank;
        else if (dé < rangedEnnemyProbThreshhold)
            return prefabDistance;
        else if (dé < normalEnnemyProbThreshhold)
            return prefabCaC;

        return null;
    }
}
