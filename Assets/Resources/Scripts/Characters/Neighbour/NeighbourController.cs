using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class NeighbourController : CharacterController
{
    public static NeighbourController instance;
    private void Awake()
    {
        instance = this;
    }

    public delegate void OnPrankEvent(GameObject instance);
    public event OnPrankEvent OnPrank;

    public int targetIndex;
    public bool fixedPosition;
    public bool fixedRotation;

    [SerializeField]
    List<NeighbourTarget> targets;


    NavMeshAgent agent;
    GameObject target;

    FieldOfView FoV;
    Transform FoVRefference;

    bool calm;
    public bool Calm { get => calm; set => calm = value; }
    public Transform Target { get => target.transform; 
                                set => target = value.gameObject; }

    new void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        //agent.updatePosition = false;
        //agent.updateRotation = false;

        FoVRefference = GameObject.FindGameObjectWithTag("FoV Reference").transform;
        FoV = FieldOfView.instance;

        targets = NeighbourTarget.GetTargets();

        target = null;
        calm = true;

        FoV.OnPrankDetected += SolveProblem;

        StartCoroutine(nameof(IdleCoroutine));
    }


    public override void Move()
    {
        UpdateParameters();
        if (!animator.GetBool("walk"))
        {
            //animator.applyRootMotion = true;
            //agent.speed = 0;
            return;
        }
        //animator.applyRootMotion = false;
        //agent.speed = 10;

        Vector3 destination = GetTargetPosition();
        //Debug.Log("set destination to " + destination + " but target is at " + target.transform.position);
        SetDestination(destination);
    }

    public override void Rotate()
    {
        UpdateFoV();

        if (fixedRotation)
            return;
        RotateToIdleTarget();

        if (!animator.GetBool("walk"))
            return;

        RotateToTarget();
    }

    public void RemoveTarget(NeighbourTarget targetToRemove)
    {
        if (target == targetToRemove)
            target = null;

        targets.Remove(targetToRemove);
        targetIndex %= targets.Count;
    }

    public void DiscoverPrank(GameObject prank)
    {
        OnPrank(prank);
    }
}
