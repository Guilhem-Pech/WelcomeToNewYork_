using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockedBackBehaviour : StateMachineBehaviour
{
    //Knockback Parameters
    [ReadOnly] public Vector3 knockBackNormalDir;
    [ReadOnly] public float knockBackStrength;
    [ReadOnly] public float knockBackDuration;

    //State Parameters
    [ReadOnly] public Rigidbody rigidbody;
    [ReadOnly] public NavMeshAgent agent;
    [ReadOnly] public float timeSinceStateEnter;
    private AnimationsReplicationBridge animBridge;
    
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Init Knockback Parameters
        knockBackNormalDir = animator.gameObject.GetComponent<TestEnnemy>().m_knockBackNormalDir;
        knockBackStrength = animator.gameObject.GetComponent<TestEnnemy>().m_knockBackStrength;
        knockBackDuration = animator.gameObject.GetComponent<TestEnnemy>().m_knockBackDuration;

        //Init State Parameters
        animBridge = animator.gameObject.GetComponent<AnimationsReplicationBridge>();
        rigidbody = animator.gameObject.GetComponent<Rigidbody>();
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        timeSinceStateEnter = 0f;

        //On active le déplacement via le rigidbody et on désactive le navmesh
        rigidbody.isKinematic = false;
        agent.enabled = false;
        animBridge.playAnimation("KnockedBack");
        rigidbody.AddForce(knockBackNormalDir * knockBackStrength * knockBackDuration);

        animator.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceStateEnter += Time.deltaTime;

        if (timeSinceStateEnter >= knockBackDuration)
            animator.SetBool("IsKnockedBack", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = false;

        //On désactive le déplacement via le rigidbody et on réactive le navmesh
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        agent.nextPosition = animator.gameObject.transform.position;
        agent.enabled = true;
        agent.SetDestination(animator.gameObject.transform.position);
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
