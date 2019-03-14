using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleBehaviour : StateMachineBehaviour
{
    private TargetingSystem targetSys;
    private SteeringSystem steerSys;
    private NavMeshAgent agent;
    private float timeSinceStateEnter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetSys = animator.gameObject.GetComponentInChildren<TargetingSystem>();
        steerSys = animator.gameObject.GetComponentInChildren<SteeringSystem>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();

        agent.speed = EnnemiParams.Instance.SeekSpeed;

        timeSinceStateEnter = 0f;

        steerSys.AllOff();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceStateEnter += Time.deltaTime;

        if (targetSys.hasTarget())
        {
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsIdle", false);
            return;
        }
        else if (/*animator.gameObject.GetComponentInParent<HordeMemberComponent>().getHorde() == null &&*/ getNearestPlayer(animator) != null)
        {
            animator.SetBool("IsSoloHunting", true);
            animator.SetBool("IsIdle", false);
        }
        else
        {
            animator.SetBool("IsWandering", true);
            animator.SetBool("IsIdle", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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

    public GameObject getNearestPlayer(Animator animator)
    {
        GameObject nearestPlayer = null;
        float nearestPlayerDistance = float.MaxValue;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (nearestPlayerDistance > (animator.gameObject.transform.position - player.transform.position).magnitude && player.GetComponent<BaseEntity>().enabled)
            {
                nearestPlayer = player;
                nearestPlayerDistance = (animator.gameObject.transform.position - player.transform.position).magnitude;
            }
        }

        return nearestPlayer;
    }
}
