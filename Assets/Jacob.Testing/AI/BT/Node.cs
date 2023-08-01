using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public abstract class Node
{
    protected NodeState m_nodeState;
    public NodeState nodeState { get { return m_nodeState; } }

    public abstract NodeState Evaluate();
}

public enum NodeState { RUNNING, PASS, FAIL, }