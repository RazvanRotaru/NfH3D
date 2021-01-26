using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NeighbourController
{
    public Quaternion targetRotation;
    public bool rotateToTarget = false;
    public float rotationSpeed = 2f;

    private void RotateToTarget()
    {
        Quaternion newRot = Quaternion.LookRotation((targetPosition
                                                - transform.position).normalized);
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            newRot = Quaternion.LookRotation(agent.velocity.normalized);

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                    newRot, Time.deltaTime * rotationSpeed);

        //transform.rotation = agent.transform.rotation;

    }

    private void UpdateFoV()
    {
        FoV.SetAimDirection(FoVRefference.forward);
        FoV.SetOrigin(FoVRefference.position);
    }

    private void RotateToIdleTarget()
    {
        if (!rotateToTarget)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                    targetRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 5f)
            rotateToTarget = false;
    }
}
