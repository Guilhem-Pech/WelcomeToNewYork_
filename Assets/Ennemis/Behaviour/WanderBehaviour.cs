using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private TargetingSystem targetSys;
    private SteeringSystem steerSys;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        targetSys = animator.gameObject.GetComponent<TargetingSystem>();
        steerSys = animator.gameObject.GetComponent<SteeringSystem>();

        agent.speed = EnnemiParams.Instance.WanderSpeed;

        steerSys.AllOff();
        steerSys.FlockingOn();
        steerSys.WanderOn();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (targetSys.hasTarget())
        {
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsWandering", false);
            return;
        }
        else
        {
            agent.SetDestination(agent.transform.position + (steerSys.Force() * agent.speed));
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        steerSys.WanderOff();
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
