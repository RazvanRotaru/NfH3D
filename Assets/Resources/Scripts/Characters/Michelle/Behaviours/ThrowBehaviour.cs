using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBehaviour : MichelleBehaviour
{
    bool thrown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.ResetTrigger("throw");
        animator.SetBool("throwing", true);
        animator.SetLayerWeight(1, 1);

        thrown = false;

        if (character == null)
            character = PlayerController.instance;

        character.HandTarget = character.GetBaloonTarget();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.35f && !thrown)
        {
            thrown = true;
            character.LaunchBaloon();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("throwing", false);
        animator.SetLayerWeight(1, 0);

        character.HandTarget = Vector3.zero;
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
    //    rotation = GetBalloonDirection();
    //    hand.rotation = rotation;
    //}
}
