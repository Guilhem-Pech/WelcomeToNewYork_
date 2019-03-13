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

    public PlayerAnimation playerAnimation;

    [ServerCallback]
    public void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
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
    protected abstract void AttackSpeciale(Vector3 playerPosition_, Vector2 vecteurDirection_);
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
}
