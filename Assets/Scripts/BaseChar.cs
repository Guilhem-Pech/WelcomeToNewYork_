using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class BaseChar : BaseEntity
{
    [SyncVar]
    public int maxStamina = 50;

    [SyncVar]
    public int currentStamina;

    public bool canMoveWhileAttacking = true;

    public PlayerAnimation playerAnimation;

    protected GameObject UI;


    public abstract void Awake();


    [ServerCallback]
    public virtual void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            UI = GameObject.FindGameObjectWithTag("UI");
        }
    }

    [Server]
    public void GainStamina(int stam)
    {
        currentStamina = currentStamina + stam;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

    [Server]
    public void UseStamina(int stam)
    {
        int minus = currentStamina - stam;
        currentStamina = minus >= 0 ? minus : 0;
    }

    [Server]
    protected abstract void AttackSpeciale(Vector3 playerPosition_, float angle);
    [Server]
    protected abstract void Attack(Vector3 point);
 

    public int getMaxStamina()
    {
        return maxStamina;
    }
    public int getStamina()
    {
        return currentStamina;
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if (UI != null)
        {
            UI.GetComponentInChildren<Life>().DisplayLife(dmg, this.GetMaxHealth());
        }
    }

    public override void AddHealth(int heal)
    {
        base.AddHealth(heal);
        if (UI != null)
        {
            UI.GetComponentInChildren<Life>().AddLife(heal);
        }
    }

    [TargetRpc]
    public void TargetAffichMort(NetworkConnection nC)
    {
        GameObject UI = GameObject.Find("UIInGame");
        UI.GetComponent<DeathScreen>().AfficherLabelMort();
    }

    [Server]
    public override void Death()
    {
        base.Death(); 
        TargetAffichMort(connectionToClient);
    }
}
