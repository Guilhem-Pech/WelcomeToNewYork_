using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class AbstractAttack : NetworkBehaviour
{
    public float cooldown = 1f;
    public float timeSinceLastAttack;

    [ServerCallback]
    private void Awake()
    {
        timeSinceLastAttack = cooldown;
    }

    [ServerCallback]
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    [Server]
    public bool IsCoolDownDone()
    {
        return timeSinceLastAttack >= cooldown;
    }

    [Server]
    public void LaunchCooldown()
    {
        timeSinceLastAttack = 0;
    }

    [Server]
    public virtual bool IsAttackPossible(GameObject player)
    {
        return IsCoolDownDone();
    }
    public abstract void Attack(GameObject player);
}
