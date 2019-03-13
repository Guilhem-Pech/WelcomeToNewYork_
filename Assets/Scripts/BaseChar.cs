using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseChar : BaseEntity
{
    public int maxStamina = 50;
    public int currentStamina;

    public PlayerAnimation playerAnimation;

    public void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
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

    protected abstract void AttackSpeciale(Vector3 playerPosition_, Vector2 vecteurDirection_);
    protected abstract void attack(Vector3 point);

    public int getMaxStamina()
    {
        return maxStamina;
    }
    public int getStamina()
    {
        return currentStamina;
    }
}
