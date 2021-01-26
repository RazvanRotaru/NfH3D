using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public delegate void InteractibleEvent(Interactible instance);
    public static event InteractibleEvent OnHover;
    public static event InteractibleEvent OnClick;
    public static event InteractibleEvent OnExit;

    //public static InventoryManager inventory;

    private void Start()
    {
        //inventory = InventoryManager.instance;
    }


    private void OnMouseOver()
    {
        if (InventoryManager.instance.CanInteract(this))
            OnHover.Invoke(this);
    }

    private void OnMouseExit()
    {
        OnExit.Invoke(this);
    }

    private void OnMouseDown()
    {
        if (InventoryManager.instance.CanInteract(this))
            OnClick.Invoke(this);
    }
}
