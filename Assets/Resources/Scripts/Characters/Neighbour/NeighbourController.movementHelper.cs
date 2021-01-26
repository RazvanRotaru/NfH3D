using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NeighbourController
{
    public float offset = 2f;
    public Vector3 targetPosition;

    private void SetDestination(Vector3 destination)
    {
        //destination.y = transform.position.y;
        agent.SetDestination(destination);
        //agent.speed = 0;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 destination;
        if (!fixedPosition)
            destination = targetPosition
                                        - transform.forward * offset;
        else
            destination = targetPosition;
        return destination;
    }

    private void UpdateParameters()
    {
        if (target == null)
            animator.SetBool("walk", false);
        if (target != null && target.CompareTag("Player"))
            UpdateTargetPosition();
    }

    public void StopMoving()
    {
        agent.ResetPath();
    }

    private void UpdateTargetPosition()
    {
        if (target == null)
            return;

        Vector3 actualPosition = target.transform.position;
        actualPosition.y = transform.position.y;
        //Debug.LogError("Setting target position at " + actualPosition);
        targetPosition = actualPosition;
    }
}
