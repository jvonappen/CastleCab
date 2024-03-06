using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {

        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.FAIL:
                    return NodeState.FAIL;
            }
        }
        return NodeState.PASS;

        //bool isNodeRunning = false;
        //foreach(var node in nodes) 
        //{
        //    switch(node.Evaluate())
        //    {
        //        case NodeState.RUNNING:
        //            isNodeRunning = true;
        //            m_nodeState = NodeState.RUNNING;
        //            return m_nodeState;
        //        case NodeState.PASS:
        //            break;
        //        case NodeState.FAIL:
        //            m_nodeState = NodeState.FAIL;
        //            return m_nodeState;
        //        default: 
        //            break;
        //    }
        //}
        //m_nodeState = isNodeRunning ? NodeState.RUNNING : NodeState.PASS;
        //return m_nodeState;
    }
}