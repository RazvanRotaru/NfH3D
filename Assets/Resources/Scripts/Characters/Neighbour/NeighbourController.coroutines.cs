using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NeighbourController
{
    IEnumerator StopCleanCoroutine()
    {
        
        yield return null;
    }

    IEnumerator CleanCoroutine()
    {
        animator.SetBool("walk", true);
        if (!InSight(targetPosition))
            yield return new WaitUntil(() => InSight(targetPosition));

        animator.SetBool("angry", true);
        yield return new WaitUntil(() => !animator.GetBool("angry"));

        if (!InReach(targetPosition))
            yield return new WaitUntil(() => InReach(targetPosition));
        agent.ResetPath();

        animator.SetBool("walk", false);
        if (target != null && target.CompareTag("Prank"))
        {
            animator.SetBool("clean", true);
            yield return new WaitUntil(() => !animator.GetBool("clean"));

            //boxCollider.enabled = true;
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
        StartCoroutine(nameof(IdleCoroutine));
    }

    IEnumerator HarmPlayerCoroutine()
    {
        StopCoroutine(nameof(CleanCoroutine));
        if (animator.GetBool("clean"))
            yield return new WaitUntil(() => !animator.GetBool("clean"));

        GameObject player = PlayerController.instance.gameObject;
        SetTarget(player);
        OnPrank(player);

        animator.SetBool("angry", true);
        //yield return new WaitUntil(() => !animator.GetBool("angry"));

        yield return new WaitUntil(() => InReach(targetPosition));
        agent.ResetPath();

        animator.SetBool("walk", false);

        Debug.Log("game lost");
        animator.SetBool("clean", true);
    }

    IEnumerator IdleCoroutine()
    {
        while (true)
        {
            //if (!animator.GetBool("is_up"))
            //    yield return new WaitUntil(() => animator.GetBool("is_up"));

            targets[targetIndex].SetActive();
            animator.SetBool("walk", true);

            yield return new WaitUntil(() => RemainingDistance() < 0.05f);

            animator.SetBool("walk", false);


            targets[targetIndex].ApplyRotation();

            yield return new WaitUntil(() => rotateToTarget == false);

            targets[targetIndex].Interact();
            targetIndex = (targetIndex + 1) % targets.Count;

            yield return new WaitForSeconds(15f);
        }

    }

    private void StopIdleCoroutine()
    {
        StopCoroutine(nameof(IdleCoroutine));
        fixedPosition = false;
        rotateToTarget = false;
    }
}
