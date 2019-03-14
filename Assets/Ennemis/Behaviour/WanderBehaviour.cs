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
        targetSys = animator.gameObject.GetComponentInChildren<TargetingSystem>();
        steerSys = animator.gameObject.GetComponentInChildren<SteeringSystem>();

        agent.speed = EnnemiParams.Instance.WalkingSpeed;

        animator.gameObject.transform.Find("Sprite").gameObject.GetComponent<Animator>().Play("Walking");
        steerSys.AllOff();
        steerSys.WanderOn();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (targetSys.hasTarget())
        {
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsWandering", false);
        }else if (getNearestPlayer(animator) != null)
        {
            animator.SetBool("IsSoloHunting", true);
            animator.SetBool("IsWandering", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
