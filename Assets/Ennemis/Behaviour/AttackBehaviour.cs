using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : StateMachineBehaviour
{
    private Animator animController;
    private NavMeshAgent agent;
    private TargetingSystem targetSys;
    private AttackSystem attackSys;
    private SteeringSystem steerSys;

    //State Parameters
    public Rigidbody rigidbody;
    public float timeSinceStateEnter;
    public bool attackStart;
    private AnimationsReplicationBridge animBridge;

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
        
        attackStart = false;

        agent.enabled = false;
        animator.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = true;

        animBridge.playAnimation("AttackIdle");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((!attackStart) && attackSys.attackAbility.IsCoolDownDone())
        {
            attackStart = true;

            animBridge.playAnimation("Attacking");
        }else if (false && (!attackStart) && (!attackSys.attackAbility.IsCoolDownDone()) && (attackSys.CanEscape && attackSys.IsTargetTooClose(targetSys.getTarget()))) { //disabled
            animController.SetBool("IsEscaping", true);
            animController.SetBool("IsAttacking", false);
        }
        else if ((!attackStart) && (!attackSys.attackAbility.IsCoolDownDone()) && !attackSys.attackAbility.IsAttackPossible(targetSys.getTarget()))
        {
            animController.SetBool("IsAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        animBridge.playAnimation("Idle");
        animator.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = false;
        agent.enabled = true;
        agent.isStopped = false;
        animator.SetBool("IsAttacking", false);
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

    public void OnAnimAttackLaunch()
    {
        attackSys.attackAbility.Attack(targetSys.getTarget());
    }
    public void OnAnimAttackEnd()
    {
        if (attackSys.CanEscape && attackSys.IsTargetTooClose(targetSys.getTarget()))
            animController.SetBool("IsEscaping", true);
        animController.SetBool("IsAttacking", false);
    }


    
}
