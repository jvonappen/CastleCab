using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    SkinData m_previewSkin, m_selectedSkin;

    ColourSelector m_colourSelector;
    private void Awake() => m_colourSelector = GetComponent<ColourSelector>();

    public void SetSkin(SO_Skin _skin) => SetSkin(_skin.m_data);

    public void SetSkin(SkinData _skin)
    {
        m_colourSelector.GetMat().SetTexture("_Skin_BaseColour", _skin.m_baseColour);
        m_colourSelector.GetMat().SetTexture("_Skin_Mask", _skin.m_mask);

        m_previewSkin = _skin;
    }

    public void ConfirmSkin() => m_selectedSkin = m_previewSkin;
    public void DisplaySelectedSkin() => SetSkin(m_selectedSkin);
}
