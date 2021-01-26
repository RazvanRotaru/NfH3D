using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourTarget : MonoBehaviour
{
    public Vector3 targetRotation;
    public Vector3 targetPosition;
    public bool willSit;

    NeighbourController neighbour;

    public static List<NeighbourTarget> GetTargets()
    {
        List<NeighbourTarget> targets = new List<NeighbourTarget>(
                                            GameObject.FindObjectsOfType<NeighbourTarget>());

        int index = targets.FindIndex((x) => x.name == "Chair");
        if (index >= 0)
        {
            NeighbourTarget aux = targets[index];
            targets[index] = targets[0];
            targets[0] = aux;
        }

        return targets;
    }

    void OnEnable()
    {
        neighbour = NeighbourController.instance;
    }

    public void SetActive()
    {
        Debug.Log("Neighbour is going to " + name);

        if (neighbour == null)
            neighbour = NeighbourController.instance;
        neighbour.Target = transform;
        neighbour.targetPosition = targetPosition;
        neighbour.fixedPosition = true;
    }

    public void ApplyRotation()
    {
        neighbour.targetRotation = Quaternion.Euler(targetRotation);
        neighbour.rotateToTarget = true;
    }

    public void Interact()
    {
        StartCoroutine(nameof(InteractCoroutine));
    }

    IEnumerator InteractCoroutine()
    {
        if (!willSit)
            yield return null;
        else
        {
            neighbour.animator.SetTrigger("sit");
            yield return new WaitUntil(() => neighbour.animator.GetBool("sitting"));
        }
    }
}
