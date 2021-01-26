using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : Interactible
{
    PlayerController player;
    CrosshairController crosshair;
    Camera cam;
    //GameObject prank;

    void Start()
    {
        player = PlayerController.instance;
        crosshair = CrosshairController.instance;
        cam = Camera.main;

        OnClick += LaunchBaloon;
        OnHover += DebugBaloon;
    }

    void LaunchBaloon(Interactible interactible)
    {
        if (interactible.GetInstanceID() == GetInstanceID())
        {
            //Debug.Log(name);
            Vector3 position = interactible.transform.position;
            RaycastHit hit;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);

            if (Physics.Raycast(ray, out hit))
                position = hit.point;

            player.ThrowBalloon(position);
        }
    }

    void DebugBaloon(Interactible interactible)
    {
        if (interactible.GetInstanceID() == GetInstanceID())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
        }
    }
}
