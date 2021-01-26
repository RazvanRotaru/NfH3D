using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NeighbourController
{
    public float sightDistance = 30f;

    public void SolveProblem(GameObject instance)
    {
        if (target != null && target.CompareTag("Player"))
            return;

        Debug.Log("seen " + instance.name/* + " at height " + instance.transform.position.y*/);

        StopIdleCoroutine();

        if (instance.CompareTag("Player"))
        {
            StartCoroutine(nameof(HarmPlayerCoroutine));
            return;
        }

        if ((target != null && target.CompareTag("Prank"))
                                || animator.GetBool("clean"))
            return;

        calm = false;
        gameObject.layer = LayerMask.NameToLayer("Neighbour");

        if (target == null || (target !=null && target.layer != LayerMask.NameToLayer("Prank")))
        {
            OnPrank(instance);
            SetTarget(instance);
            StartCoroutine(nameof(CleanCoroutine));
        }
    }

    private float RemainingDistance()
    {
        return Vector3.SqrMagnitude(transform.position - targetPosition);
    }

    bool InSight(Vector3 target)
    {
        float distance = Vector3.SqrMagnitude(transform.position - target);
        return distance < sightDistance;
    }

    public void Fall()
    {
        animator.SetTrigger("fall");
    }

    public void SetTarget(GameObject someObject)
    {
        target = someObject;

        UpdateTargetPosition();
    }

    public void DestroyTarget()
    {
        if (target == null)
        {
            //StartCoroutine(nameof(IdleCoroutine));
            return;
        }

        if (!target.CompareTag("Player"))
            Destroy(target);
        else
            PlayerController.instance.Lose();
        target = null;
    }
}
