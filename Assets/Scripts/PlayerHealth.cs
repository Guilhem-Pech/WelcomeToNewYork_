using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [Range(50,150)]
    public int maxHealth = 100 ;
    public int currentHealth ;
    // Start is called before the first frame update

    void GetAttack(int hit)
    {
        currentHealth = currentHealth - hit;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth ;
        print(currentHealth);
    }

    void Start()
    {
        currentHealth = maxHealth;
        print(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            GetAttack(50) ;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetAttack(-50);
        }
    }
 
}
