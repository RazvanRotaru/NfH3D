using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Interactible
{
    public Item item;
    PlayerController player;
    InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        inventory = InventoryManager.instance;

        gameObject.tag = "ItemContainer";

        OnClick += PickUpItem;
    }

    public void PickUpItem(Interactible interactible)
    {
        if (interactible.GetInstanceID() == GetInstanceID()
                                && item != null)
            if (player.InReach(transform.position))
            {
                inventory.AddItem(item);
                item = null;
            }
    }

    public bool HasItem()
    {
        return item != null;
    }
}
