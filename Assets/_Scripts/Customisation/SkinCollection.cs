using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCollection : MonoBehaviour
{
    [SerializeField] SkinSelector m_skinSelector;
    public SkinSelector skinSelector { get { return m_skinSelector; } }
}
