using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSettings : MonoBehaviour
{
    Texture m_baseColour;
    Texture m_maskMap;

    public Texture baseColour { get { return m_baseColour; } }
    public Texture maskMap { get { return m_maskMap; } }

    Material m_defaultMat;
    public Material defaultMat { get { return m_defaultMat; } }

    private void Awake() => Init();

    bool m_isInitialised;
    public void Init()
    {
        if (m_isInitialised) return;

        m_defaultMat = GetComponent<Renderer>().sharedMaterial;

        if (m_defaultMat.HasProperty("_Base_Colour")) m_baseColour = m_defaultMat.GetTexture("_Base_Colour");
        if (m_defaultMat.HasProperty("_Mask_Map")) m_maskMap = m_defaultMat.GetTexture("_Mask_Map");

        m_isInitialised = true;
    }
}
