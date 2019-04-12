using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscapeBehaviour : StateMachineBehaviour
{
    private Animator animController;
    private NavMeshAgent agent;
    private TargetingSystem targetSys;
    private AttackSystem attackSys;
    private SteeringSystem steerSys;
    private AnimationsReplicationBridge animBridge;

    Vector3 WarpPosition;

    // onstateenter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        animController = animator;
        animBridge = animator.gameObject.GetComponent<AnimationsReplicationBridge>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        targetSys = animator.gameObject.GetComponentInChildren<TargetingSystem>();
        steerSys = animator.gameObject.GetComponentInChildren<SteeringSystem>();
        attackSys = animator.gameObject.GetComponentInChildren<AttackSystem>();

        steerSys.AllOff();
        agent.speed = 0;
        agent.ResetPath();
        agent.isStopped = true;
        agent.SetDestination(animator.gameObject.transform.position);

        agent.enabled = false;
        animator.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = true;

        animBridge.playAnimation("Teleport");

        //Calcul de la position idéal de la téléportation
        Vector3 agentPos = animator.gameObject.transform.position;
        Vector3 directionToTarget = (targetSys.getTarget().transform.position - agentPos).normalized;
        WarpPosition = ((-directionToTarget) * attackSys.escapeRange) + agentPos;

        //On vérifie à l'aide d'un raycast si la TP ne fait pas sortir du navmesh
        NavMeshHit hit;
        if ((NavMesh.Raycast(agentPos, WarpPosition, out hit, NavMesh.AllAreas)))
        { //Si c'est le cas, on ajuste la position de la tp
            WarpPosition = hit.position;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animBridge.playAnimation("Idle");
        animator.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = false;
        agent.enabled = true;
        agent.isStopped = false;
        animator.SetBool("IsEscaping", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    public void OnAnimTeleportLaunch()
    {
        agent.Warp(WarpPosition);
    }
    public void OnAnimTeleportEnd()
    {
        animController.SetBool("IsEscaping", false);
    }
}
