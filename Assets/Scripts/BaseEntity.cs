using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class BaseEntity : NetworkBehaviour {

    [SyncVar] //Need to be synchronised bot not to all clients like it does right now (= Can be optimized)
    public int maxHealth = 100;

    [SyncVar]
    public int currentHealth ;

    public bool isHit = false;

    protected float tps = 1;

    [Server]
    public void TakeDamage(int dmg)
    {
        RpcTakeDamage();
        currentHealth = currentHealth - dmg;
        if (currentHealth <= 0)
            Death();
    }

    [ClientRpc]
    public void RpcTakeDamage()
    {
        isHit = true;
    }

    [Server]
    public void AddHealth(int heal)
    {
        currentHealth = currentHealth + heal ;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    // Update is called once per frame
    [ClientCallback]
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

    [Server]
    public virtual void Death()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        gm.GetComponent<GameManager>().playerMan.Death(this.gameObject);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetHealth()
    {
        return currentHealth;
    }
}
