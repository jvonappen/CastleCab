using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CosmeticList", menuName = "CosmeticList")]
public class SO_Cosmetics : ScriptableObject
{
    public List<SO_Dye> m_dyes;

    public List<Texture2D> m_patterns;
    public List<SO_Skin> m_outfits;
}
