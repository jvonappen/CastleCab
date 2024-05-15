using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    ColourSelector m_colourSelector;
    private void Awake() => m_colourSelector = GetComponent<ColourSelector>();

    public void SetSkin(SO_Skin _skin) => SetSkin(_skin.m_data);

    public void SetSkin(SkinData _skin)
    {
        m_colourSelector.GetMat().SetTexture("_Skin_BaseColour", _skin.m_baseColour);
        m_colourSelector.GetMat().SetTexture("_Skin_Mask", _skin.m_mask);
    }
}
