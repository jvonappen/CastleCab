using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSettings : MonoBehaviour
{
    [SerializeField] bool m_useBaseColour;
    [ConditionalHide("m_useBaseColour")] [SerializeField] Texture m_baseColour;

    [SerializeField] Texture m_maskMap;

    public bool useBaseColour { get { return m_useBaseColour; } }
    public Texture baseColour { get { return m_baseColour; } }
    public Texture maskMap { get { return m_maskMap; } }
}
