using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    private GameObject ui;

    public RangeNode(float range, Transform target, Transform origin, GameObject ui)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
        this.ui = ui;
    }

    public override NodeState Evaluate()
    {

        float distance = Vector3.Distance(target.position, origin.position);
        if (distance <= range)
        { if (ui != null) ui.SetActive(true); }
        else
        { if (ui != null) ui.SetActive(false); }

        return distance <= range ? NodeState.PASS : NodeState.FAIL;
    }
}