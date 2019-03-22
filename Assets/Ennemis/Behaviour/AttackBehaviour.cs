using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : StateMachineBehaviour
{
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

        animBridge.playAnimation("Attacking");
        timeSinceStateEnter = 0f;
        attackStart = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceStateEnter += Time.deltaTime;

        if (!attackStart && timeSinceStateEnter > 0.1f)
        {
            attackStart = true;

            foreach (GameObject player in attackSys.getTargetList())
            {
                player.GetComponentInParent<BaseChar>().TakeDamage(10);
            }
        }
        else if (attackStart && timeSinceStateEnter > 0.5f)
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        agent.isStopped = false;
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
