using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    float prevh = 0f;

    void UpdateParams()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical") * moonwalk;
        
        changedDirection = prevh * h < 0;
        prevh = h;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            playerSpeed = (int)Speed.RunningSpeed;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            playerSpeed = (int)Speed.WalkingSpeed;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            inventory.SelectItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            inventory.SelectItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            inventory.SelectItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            inventory.SelectItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            inventory.SelectItem(4);
    }
    void ChangeAnimation(int musicID)
    {
        animator.SetInteger("music_ID", musicID % animator.GetInteger("known_dances") + 1);
    }

    public void ThrowBalloon(Vector3 target)
    {
        baloonTarget = target;
        animator.SetTrigger("throw");
        Debug.DrawLine(transform.position, target, Color.white, 1f);
    }

    public void LaunchBaloon()
    {
        GameObject baloon = Instantiate(baloonPrefab);
        baloon.GetComponent<BaloonController>()
                                .Init(throwOrigin, baloonTarget);
        baloonTarget = Vector3.zero;
    }

    public Vector3 GetBaloonTarget()
    {
        return baloonTarget;
    }
}
