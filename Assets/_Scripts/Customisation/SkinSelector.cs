using UnityEngine;

public class SkinSelector : CustomisationSelector
{
    SkinData m_previewSkin, m_selectedSkin;

    ColourSelector m_colourSelector;

    bool m_isInitialised;

    private void Start() => Init();
    public void Init()
    {
        if (m_isInitialised) return;

        m_colourSelector = GetComponent<ColourSelector>();
        m_selectedSkin = GetSkin();

        m_isInitialised = true;
    }

    public SkinData GetSkin()
    {
        SkinData skin;
        skin.m_baseColour = m_colourSelector.GetMat().GetTexture("_Skin_BaseColour") as Texture2D;
        skin.m_mask = m_colourSelector.GetMat().GetTexture("_Skin_Mask") as Texture2D;

        return skin;
    }

    public SkinData GetSelectedSkin() => m_selectedSkin;

    public void SetSkin(SO_Skin _skin) => SetSkin(_skin.m_data);

    public void SetSkin(SkinData _skin)
    {
        if (!m_colourSelector) m_colourSelector = GetComponent<ColourSelector>();

        m_colourSelector.GetMat().SetTexture("_Skin_BaseColour", _skin.m_baseColour);
        m_colourSelector.GetMat().SetTexture("_Skin_Mask", _skin.m_mask);

        m_previewSkin = _skin;
    }

    public void ConfirmSkin() => m_selectedSkin = m_previewSkin;
    public override void DisplaySelected() => SetSkin(m_selectedSkin);
}
