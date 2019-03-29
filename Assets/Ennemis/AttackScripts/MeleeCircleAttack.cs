using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MeleeCircleAttack : AbstractAttack
{
    public int Damage;

    [Server]
    public override void Attack(GameObject player)
    {
        foreach (GameObject target in this.gameObject.GetComponent<AttackSystem>().getTargetList())
        {
            target.GetComponentInParent<BaseChar>().TakeDamage(Damage);
        }
        LaunchCooldown();
    }

    [Server]
    public override bool IsAttackPossible(GameObject player)
    {
        return this.gameObject.GetComponent<AttackSystem>().IsAlreadyTarget(player);
    }
}
