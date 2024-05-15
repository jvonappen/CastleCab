using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "HorseSkin")]
public class SO_Skin : ScriptableObject
{
    public SkinData m_data;
}

[System.Serializable]
public struct SkinData
{
    public Texture2D m_baseColour, m_mask;
}
