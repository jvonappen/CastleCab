using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dye", menuName = "Dye")]
public class SO_Dye : ScriptableObject
{
    public Color m_colour;
    [Range(0, 1)] public float m_metal;
    [Range(0, 1)] public float m_roughness;

    public DyeData GetDyeData()
    {
        return new DyeData() { colour = m_colour, metal = m_metal, roughness = m_roughness };
    }
}
