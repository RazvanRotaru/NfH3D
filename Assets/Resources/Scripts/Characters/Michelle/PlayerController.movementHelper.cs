using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private Vector3 GetMoveDirection()
    {
        Vector3 moveDirection = v * moonwalk * camTransform.forward
                                            + h * camTransform.right;
        //moveDirection.z *= moonwalk;
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized /** animator.deltaPosition.magnitude*/;
        return moveDirection;
    }

    private void ApplyVelocity(Vector3 moveDirection)
    {
        Vector3 deltaVelocity = moveDirection / Time.deltaTime;
        deltaVelocity *= playerSpeed * speedModifier * animator.deltaPosition.magnitude;
        float rbVelY = rigidbody.velocity.y;

        rigidbody.velocity = new Vector3(deltaVelocity.x, rbVelY, deltaVelocity.z);

        //rigidbody.velocity = deltaVelocity;

        //rigidbody.velocity = moveDirection / (Time.deltaTime + 0.0001f)
        //* animator.deltaPosition.magnitude;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
