using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class RangedAttack : AbstractAttack
{
    public GameObject Projectile;

    [Server]
    public override void Attack(GameObject player)
    {
        if (player == null)
            return;

        GameObject shooter = this.transform.parent.gameObject; //on récupère le game object de l'ennemi
        Vector3 shooterPos = shooter.transform.position; // position de l'ennemi
        
        //On calcule l'angle entre l'ennemi et le joueur
        Vector3 shooterToTarget = player.transform.position - shooter.transform.position;
        float angle = Vector3.SignedAngle(shooterToTarget, Vector3.right,Vector3.up); // on récupère l'angle de la main pour avoir l'angle de tir

        shooterPos.y = 0.6f;
        GameObject theShot = Instantiate(Projectile); // On instantie le projectile
        NetworkServer.Spawn(theShot);

        
        theShot.GetComponent<Projectile>().initialisation(angle, shooterPos);
        LaunchCooldown();
    }

    [Server]
    public override bool IsAttackPossible(GameObject player)
    {
        if(!gameObject.GetComponent<AttackSystem>().IsAlreadyTarget(player))
            return false;

        NavMeshHit hit;

        bool IsLineOfSightClear = !(NavMesh.Raycast(transform.position, player.transform.position, out hit, NavMesh.AllAreas));

        return IsLineOfSightClear;
    }
}
