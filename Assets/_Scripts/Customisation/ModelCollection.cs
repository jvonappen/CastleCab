using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCollection : MonoBehaviour
{
    [SerializeField] ModelSelector m_modelSelector;
    public ModelSelector modelSelector { get { return m_modelSelector; } }
}
