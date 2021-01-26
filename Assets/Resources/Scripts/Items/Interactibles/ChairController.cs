using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : Interactible
{
    PlayerController player;
    NeighbourController neighbour;

    public bool willSting;

    void Start()
    {
        player = PlayerController.instance;
        neighbour = NeighbourController.instance;
        willSting = false;

        OnClick += ActivatePrank;
    }

    void ActivatePrank(Interactible interactible)
    {
        if (interactible.GetInstanceID() == GetInstanceID())
            if (player.InReach(transform.position))
            {
                willSting = true;
                InventoryManager.instance.RemoveCurrentItem();
            }
    }

    public void Sting()
    {
        gameObject.tag = "Prank";
        neighbour.SetTarget(null);
        neighbour.SolveProblem(gameObject);
    }

    private void OnDestroy()
    {
        neighbour.RemoveTarget(GetComponent<NeighbourTarget>());
    }
}
