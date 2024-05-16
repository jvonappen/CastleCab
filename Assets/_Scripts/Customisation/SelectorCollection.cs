using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorCollection : MonoBehaviour
{
    [SerializeField] CustomisationSelector m_selector;
    public CustomisationSelector selector { get { return m_selector; } }
}
