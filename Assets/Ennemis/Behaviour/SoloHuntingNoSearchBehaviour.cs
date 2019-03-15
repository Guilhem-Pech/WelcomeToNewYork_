using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoloHuntingNoSearchBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private TargetingSystem targetSys;
    private SteeringSystem steerSys;
    private AnimationsReplicationBridge animBridge;
    
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animBridge = animator.gameObject.GetComponent<AnimationsReplicationBridge>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        targetSys = animator.gameObject.GetComponentInChildren<TargetingSystem>();
        steerSys = animator.gameObject.GetComponentInChildren<SteeringSystem>();

        agent.speed = EnnemiParams.Instance.SeekSpeed;

        steerSys.AllOff();
        GameObject nearestPlayer = getNearestPlayer(animator);
        if (nearestPlayer != null)
        {
            animBridge.playAnimation("SeekingPrey");

            steerSys.SetSeekPos(nearestPlayer.transform.position);
            steerSys.SeekOn();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (targetSys.hasTarget()) //Si il a une cible
        { //On passe dans l'état Chasing
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsSoloHunting", false);
            return;
        }

        Horde horde = animator.gameObject.GetComponentInParent<HordeMemberComponent>().getHorde(); ;
        if (false && horde != null) //Si il existe une horde
        {   //On sort de l'état
            animator.SetBool("IsSoloHunting", false);
        }
        else //Sinon il n'existe pas de horde proche
        { //On se dirige vers le joueur le plus proche
            GameObject nearestPlayer = getNearestPlayer(animator);
            if (nearestPlayer != null)
            {
                steerSys.SetSeekPos(nearestPlayer.transform.position);
                steerSys.SeekOn();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        steerSys.AllOff();
        animator.SetBool("IsSoloHunting", false);
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
