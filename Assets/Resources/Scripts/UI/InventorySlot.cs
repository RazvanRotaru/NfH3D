using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    Image image;
    Image highlight;
    Transform doneImage;
    public bool done = false;

    Color defaultColor;
    Color defaultHighlightColor;

    private void Start()
    {
        image = transform.Find("Image").GetComponent<Image>();
        highlight = GetComponent<Image>();
        doneImage = transform.Find("Done");

        defaultColor = image.color;
        defaultHighlightColor = highlight.color;
    }


    public void SetItem(Item item)
    {
        this.item = item;

        image.color = Color.white;
        image.sprite = item.sprite;
    }

    public void RemoveItem()
    {
        item = null;
        
        image.color = defaultColor;
        image.sprite = null;
    }

    public void SelectItem()
    {
        if (item == null || done)
            return;

        highlight.color = Color.white;
    }

    public void DeselectItem()
    {
        highlight.color = defaultHighlightColor;
    }

    public void ItemDone()
    {
        DeselectItem();
        done = true;
        doneImage.gameObject.SetActive(true);
    }

    public bool CanInteractWith(Interactible interactible)
    {
        if (item == null || done)
            return false;

        return interactible.name.Contains(item.canInteractWith);
    }
}
