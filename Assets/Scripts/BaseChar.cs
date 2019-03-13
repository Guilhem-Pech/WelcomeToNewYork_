using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseChar : BaseEntity
{
    public int maxStamina = 50;
    public int currentStamina;

    public bool canMoveWhileAttacking = true;

    public PlayerAnimation playerAnimation;

    protected GameObject UI;

    public void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            UI = GameObject.FindGameObjectWithTag("UI");
        }
    }

    public void gainStamina(int stam)
    {
        currentStamina = currentStamina + stam;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

    public void useStamina(int stam)
    {
        currentStamina = currentStamina - stam;
    }

    protected abstract void AttackSpeciale(Vector3 playerPosition_, float vecteurDirection_);
    protected abstract void attack(Vector3 point);

    public int getMaxStamina()
    {
        return maxStamina;
    }
    public int getStamina()
    {
        return currentStamina;
    }

    public override void takeDamage(int dmg)
    {
        base.takeDamage(dmg);
        if (UI != null)
        {
            UI.GetComponentInChildren<Life>().DisplayLife(dmg);
        }
    }

    public override void getHeal(int heal)
    {
        base.getHeal(heal);
        if (UI != null)
        {
            UI.GetComponentInChildren<Life>().AddLife(heal);
        }
    }
}
