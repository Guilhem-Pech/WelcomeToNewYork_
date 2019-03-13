using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PackHuntingBehaviour : StateMachineBehaviour
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

        agent.speed = EnnemiParams.Instance.ChaseSpeed;

        steerSys.AllOff();
        steerSys.FlockingOn();
        steerSys.SetSeekPos(animator.gameObject.GetComponent<HordeMemberComponent>().getHorde().GetHuntPos());
        steerSys.SeekOn();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Horde horde = animator.gameObject.GetComponent<HordeMemberComponent>().getHorde();
        if (targetSys.hasTarget())
        {
            animator.SetBool("IsChasing", true);
            return;
        }
        else if (horde == null)
        {
            animator.SetBool("IsPackHunting", false);
        }
        else
        {
            steerSys.SetSeekPos(animator.gameObject.GetComponent<HordeMemberComponent>().getHorde().GetHuntPos());
            agent.SetDestination(agent.transform.position + (steerSys.Force() * agent.speed));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        steerSys.SeekOff();
        animator.SetBool("IsPackHunting", false);
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
