using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : Interactible
{
    PlayerController player;
    DisplayController display;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        display = transform.Find("Display").GetComponent<DisplayController>();

        OnClick += ActivatePrank;
    }

    void ActivatePrank(Interactible interactible)
    {
        if (interactible.GetInstanceID() == GetInstanceID())
            if (player.InReach(transform.position))
            {
                display.Break();
                gameObject.tag = "Prank";
                InventoryManager.instance.RemoveCurrentItem();
            }
    }
}
