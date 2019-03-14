using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HordeSearchingBehaviour : StateMachineBehaviour
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

        agent.speed = EnnemiParams.Instance.ChaseSpeed;

        steerSys.AllOff();
        Horde nearestHorde = HordesManager.Instance.getNearestHordeFromPos(animator.gameObject.transform.position);
        steerSys.SetSeekPos(nearestHorde.getGlobalCenter());
        steerSys.SeekOn();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (targetSys.hasTarget()) //Si il a une cible
        { //On passe dans l'état Chasing
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsHordeSearching", false);
            return;
        }

        Horde horde = animator.gameObject.GetComponentInParent<HordeMemberComponent>().getHorde();
        if (horde == null) //Si il n'a pas de horde
        {   //On check la horde la plus proche + si il reste une horde active
            Horde nearestHorde = HordesManager.Instance.getNearestHordeFromPos(animator.gameObject.transform.position);
            if (nearestHorde == null) //Si pas de horde active
            {   //Il passe en état SoloHunting
                animator.SetBool("IsSoloHunting", true);
            }
            else //Sinon
            {   //Il seek le centre la horde la plus proche
                steerSys.SetSeekPos(nearestHorde.getGlobalCenter());
            }
        }
        else //Sinon si il a une horde
        {   //On sort de cet état
            animator.SetBool("IsHordeSearching", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        steerSys.AllOff();
        animator.SetBool("IsHordeSearching", false);
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
