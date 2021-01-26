using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public BoxCollider boxCollider;

    public float reachableDistance = 3f;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    protected void LateUpdate()
    {
        Move();
        Rotate();
    }

    public abstract void Move();
    public abstract void Rotate();

    public bool InReach(Vector3 target)
    {
        float distance = Vector3.SqrMagnitude(transform.position - target);
        return distance < reachableDistance;
    }
}
