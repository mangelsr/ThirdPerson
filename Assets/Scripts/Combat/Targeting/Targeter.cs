using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }


    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();
        if (target != null) targets.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        Target target = other.GetComponent<Target>();
        if (target != null) targets.Remove(target);

        // Another way to implement it using TryGetComponent 
        // which returns a boolean an the out variable to store the result 

        // if (other.TryGetComponent(out Target target1))
        //     targets.Remove(target1);
    }

    public bool CanSelectTarget()
    {
        if (targets.Count == 0) return false;
        CurrentTarget = targets[0];
        return true;
    }

    public void Cancel()
    {
        CurrentTarget = null;
    }
}
