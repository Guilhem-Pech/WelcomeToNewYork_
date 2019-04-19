using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour {

    private NavMeshAgent agent;
    private TargetingSystem targetSys;
    private AttackSystem attackSys;
    private SteeringSystem steerSys;
    private AnimationsReplicationBridge animBridge;
    
    

    // onstateenter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        animBridge = animator.gameObject.GetComponent<AnimationsReplicationBridge>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        targetSys = animator.gameObject.GetComponentInChildren<TargetingSystem>();
        steerSys = animator.gameObject.GetComponentInChildren<SteeringSystem>();
        attackSys = animator.gameObject.GetComponentInChildren<AttackSystem>();

        agent.speed = animator.gameObject.GetComponent<TestEnnemy>().ChaseSpeed;

        steerSys.AllOff();

        if (targetSys.hasTarget())
        {
            animBridge.playAnimation("Chasing");
            steerSys.PursuitOn(targetSys.getTarget());
        }
            
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!targetSys.hasTarget())
        {
            animator.SetBool("IsChasing", false);
        }else if(attackSys.getCount() > 0 && attackSys.attackAbility.IsAttackPossible(targetSys.getTarget()))
        {
            animator.SetBool("IsAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        steerSys.AllOff();
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
}
