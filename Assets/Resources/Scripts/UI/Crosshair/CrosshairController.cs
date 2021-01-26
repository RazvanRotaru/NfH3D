using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public static CrosshairController instance;

    private void Awake()
    {
        instance = this;
    }
    Image image;
    Camera cam;
    InventoryManager inventory;

    public float maxDistance = 30f;

    public delegate void CrosshairEvent(GameObject instance);
    public event CrosshairEvent OnHover;
    public event CrosshairEvent OnClick;

    void Start()
    {
        image = GetComponent<Image>();
        cam = Camera.main;

        inventory = InventoryManager.instance;

        Interactible.OnExit += ResetColor;
        Interactible.OnHover += ChangeColor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + Vector3.forward;
        //Vector3 mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        //RaycastHit hit;
        //Debug.DrawRay(cam.transform.position, (mousePosition - cam.transform.position) * 20, Color.red);

        //if (Physics.Raycast(transform.position, (mousePosition - cam.transform.position),
        //                            out hit, maxDistance, LayerMask.GetMask("Interactible")))
        //{
        //    image.color = Color.red;
        //}
        //else
        //    image.color = Color.white;
    }

    public void ChangeColor(Interactible dummy)
    {
        image.color = Color.yellow;
    }

    public void ResetColor(Interactible dummy)
    {
        image.color = Color.white;
    }
}