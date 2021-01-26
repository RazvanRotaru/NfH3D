using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private void Awake()
    {
        instance = this;
    }

    int index = -1;
    int currItem = -1;
    Transform content;
    public InventorySlot[] inventorySlots;

    private void Start()
    {
        content = GameObject.FindGameObjectWithTag("InventoryContent").transform;
        inventorySlots = new InventorySlot[content.childCount];

        int i = 0;
        foreach (Transform child in content)
            inventorySlots[i++] = child.GetComponent<InventorySlot>();
    }

    public void AddItem(Item item)
    {
        inventorySlots[++index].SetItem(item);
    }

    public void SelectItem(int index)
    {
        currItem = index;

        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            if (i == currItem)
                inventorySlots[i].SelectItem();
            else
                inventorySlots[i].DeselectItem();
        }
    }

    public bool CanInteract(Interactible interactible)
    {
        if (interactible.CompareTag("ItemContainer") 
                          && ((ItemContainer) interactible).HasItem())
            return true;

        if (currItem < 0)
            return false;

        return inventorySlots[currItem].CanInteractWith(interactible);
    }

    public void RemoveCurrentItem()
    {
        inventorySlots[currItem].ItemDone();
        currItem = -1;
    }
}
