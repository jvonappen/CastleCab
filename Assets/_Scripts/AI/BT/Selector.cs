using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> nodes = new List<Node>();

    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    m_nodeState = NodeState.RUNNING;
                    return m_nodeState;
                case NodeState.PASS:
                    m_nodeState = NodeState.PASS;
                    return m_nodeState;
                case NodeState.FAIL:
                    break;

                default:
                    break;
            }
        }
        m_nodeState = NodeState.FAIL;
        return m_nodeState;
    }
}