using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invertor : Node
{
    protected Node node;

    public Invertor(Node node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.RUNNING:
                m_nodeState = NodeState.RUNNING;
                break;
            case NodeState.PASS:
                m_nodeState = NodeState.FAIL;
                break;
            case NodeState.FAIL:
                m_nodeState = NodeState.PASS;
                break;
            default:
                break;
        }
        return m_nodeState;
    }
}