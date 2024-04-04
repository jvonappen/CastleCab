using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSettings : MonoBehaviour
{
    [SerializeField] Texture m_baseColour;
    [SerializeField] Texture m_maskMap;

    public Texture baseColour { get { return m_baseColour; } }
    public Texture maskMap { get { return m_maskMap; } }
}
