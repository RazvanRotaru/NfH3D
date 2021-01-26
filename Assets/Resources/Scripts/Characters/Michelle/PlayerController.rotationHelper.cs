using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    Quaternion targetRotation = Quaternion.identity;
    Transform head;
    Transform hand;

    Vector3 handTarget = Vector3.zero;

    Transform camTransform;
    bool cameraRotationPending = false;
    bool changedDirection = false;

    public Vector3 HandTarget { set => handTarget = value; }

    private void FreeRotation()
    {
        Vector3 lookDir = transform.forward;
        if (moveDirection.magnitude > 10e-3f)
            lookDir = moveDirection * moonwalk;
        //lookDir.x *= moonwalk;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                    Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);


        cameraRotationPending = moveDirection.magnitude > 10e-3f;
    }

    private bool ShouldTurn()
    {
        return v * moonwalk * transform.forward.z * camTransform.forward.z < 0;
    }

    public void Turn()
    {
        targetRotation = Quaternion.AngleAxis(180, transform.up) * transform.rotation;
    }

    public void RotateHead(Quaternion rotation, float percent)
    {
        /*
        rotation = Quaternion.Euler(0, 90, 0);
        if (Quaternion.Angle(camTransform.rotation, transform.rotation) > 120 || percent < 0)
            rotation = Quaternion.Inverse(rotation);
        rotation = head.rotation * rotation;
        //head.rotation = rotation * head.rotation;
        Debug.Log(percent);
        head.rotation = Quaternion.Slerp(head.rotation, rotation, percent);
        */

        if (Quaternion.Angle(camTransform.rotation, transform.rotation) > 120)
            rotation = Quaternion.Inverse(rotation);

        //head.rotation = rotation * head.rotation;
        head.rotation = Quaternion.Slerp(head.rotation, rotation * head.rotation, percent);
    }

    public Quaternion GetRotation()
    {
        if (moonwalk < 0)
        {
            if (cameraRotationPending && v * moonwalk < 0)
                return transform.rotation;

            return Quaternion.AngleAxis(180, transform.up) 
                                            * transform.rotation;
        }

        if (cameraRotationPending && v * moonwalk < 0)
            return Quaternion.AngleAxis(180, transform.up)
                                             * transform.rotation;

        return transform.rotation;
    }

    void RotateaHand()
    {
        if (handTarget == Vector3.zero)
            return;

        Vector3 handPosition = hand.position;
        Vector3 dir = handTarget - handPosition;

        hand.rotation = Quaternion.Lerp(hand.rotation, Quaternion.LookRotation(dir, Vector3.up), Time.deltaTime * 10f);
    }
}
