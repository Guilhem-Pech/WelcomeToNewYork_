using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth ;
    public bool isHit = false;
    protected float tps = 1;

    public void takeDamage(int dmg)
    {
        isHit = true;
        currentHealth = currentHealth - dmg;
        if (currentHealth <= 0)
            death();
    }

    public void getHeal(int heal)
    {
        currentHealth = currentHealth + heal ;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    // Update is called once per frame
    public virtual void Update()
    {

        if (isHit)
        {
            tps -= Time.deltaTime;

            foreach (SpriteRenderer i in this.GetComponentsInChildren<SpriteRenderer>())
                i.color = Color.red;
            

            if (tps <= 0)
            {
                isHit = false;
                tps = 1;
                foreach (SpriteRenderer i in this.GetComponentsInChildren<SpriteRenderer>())
                    i.color = Color.white;
            }
        }
    }
    public virtual void death()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        gm.GetComponent<GameManager>().playerMan.death(this.gameObject);
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }
    public int setHeal()
    {
        return currentHealth;
    }
}
