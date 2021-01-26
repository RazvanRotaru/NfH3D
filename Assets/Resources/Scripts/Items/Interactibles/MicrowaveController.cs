using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveController : Interactible
{
    PlayerController player;
    Transform prank;
    void Start()
    {
        player = PlayerController.instance;

        prank = transform.Find("Prank");
        prank.gameObject.SetActive(false);

        OnClick += ActivatePrank;
    }

    void ActivatePrank(Interactible interactible)
    {
        if (interactible.GetInstanceID() == GetInstanceID()
                                && !prank.gameObject.activeSelf)
            if (player.InReach(transform.position))
            {
                prank.gameObject.SetActive(true);
                gameObject.tag = "Prank";
                InventoryManager.instance.RemoveCurrentItem();
            }
    }
}
