using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : CharacterController
{
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    enum Speed
    {
        WalkingSpeed = 1,
        RunningSpeed = 2
    }
    int playerSpeed;
    float speedModifier = 1;
    int moonwalk = 1;

    public float rotationLimit = 90f;
    public float rotationSpeed = 10f;

    float v, h;

    InventoryManager inventory;

    Transform throwOrigin;
    GameObject baloonPrefab;
    Vector3 baloonTarget;
    Vector3 moveDirection;

    bool defeated = false;

    public int Moonwalk { get => moonwalk; set => moonwalk = value; }
    public float SpeedModifier { get => speedModifier; set => speedModifier = value; }

    new void Start()
    {
        base.Start();

        hand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        head = GameObject.FindGameObjectWithTag("PlayerHead").transform;

        throwOrigin = GameObject.FindGameObjectWithTag("Throw Origin").transform;
        baloonPrefab = Resources.Load<GameObject>("Models/WaterBallon/Balloon/Baloon");

        inventory = InventoryManager.instance;

        playerSpeed = (int)Speed.WalkingSpeed;
        camTransform = Camera.main.transform;
        MusicController.instance.OnSongChange += ChangeAnimation;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //    animator.SetInteger("music_ID", 1);

        if (defeated)
            return;

        UpdateParams();
    }

    public override void  Move()
    {
        moveDirection = GetMoveDirection();
        animator.SetBool("walk", moveDirection.magnitude != 0);

        ApplyVelocity(moveDirection);
    }

    public override void Rotate()
    {
        FreeRotation();
        RotateaHand();
/*
        if (animator.GetBool("turning"))
        {
            ResetRotation();
            return;
        }

        FreeRotation();
        if (ShouldTurn())
        {
            Debug.Log("turn");
            animator.SetTrigger("turn");
        }
*/
    }

    private void OnDisable()
    {
        MusicController.instance.OnSongChange -= ChangeAnimation;
    }

    public void Lose()
    {
        defeated = true;
        gameObject.layer = LayerMask.NameToLayer("Default");

        animator.SetTrigger("defeated");
    }
}
