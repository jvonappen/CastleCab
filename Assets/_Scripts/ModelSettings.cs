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

    private void Awake()
    {
        m_defaultMat = GetComponent<Renderer>().sharedMaterial;

        try
        {
            m_baseColour = m_defaultMat.GetTexture("_Base_Colour");
            m_maskMap = m_defaultMat.GetTexture("_Mask_Map");
        }
        catch
        {
            Debug.LogWarning("Incorrect shader is being used on object: " + gameObject);
        }
    }
}
