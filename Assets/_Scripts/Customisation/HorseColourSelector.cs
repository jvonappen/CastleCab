using UnityEngine;

public class HorseColourSelector : ColourSelector
{
    SkinSelector m_skinSelector;
    public SkinSelector skinSelector { get { return m_skinSelector; } }


    Texture2D m_previewPattern, m_selectedPattern;
    public Texture2D GetSelectedPattern() => m_selectedPattern;

    Material m_defaultMat, m_currentMat;

    bool m_hasInitialised;

    private void Awake() => Init();
    public void Init()
    {
        if (m_hasInitialised) return;

        m_defaultMat = GetComponent<Renderer>().sharedMaterial;
        m_selectedPattern = m_defaultMat.GetTexture("_Horse_Pattern") as Texture2D;

        m_skinSelector = GetComponent<SkinSelector>();

        // Instances material
        m_currentMat = new(m_defaultMat);
        GetComponent<Renderer>().sharedMaterial = m_currentMat;

        m_hasInitialised = true;
    }

    public void SetPattern(Texture2D _pattern)
    {
        m_currentMat.SetTexture("_Horse_Pattern", _pattern);
        m_previewPattern = _pattern;
    }

    public void ConfirmPattern() => m_selectedPattern = m_previewPattern;
    public override void DisplaySelected() => SetPattern(m_selectedPattern);

    public void SetDyes(HorseMatInformation _data)
    {
        SetDye("Base", _data.baseDye);
        SetDye("Hair", _data.hairDye);
        SetDye("Tail", _data.tailDye);
        SetDye("Nose", _data.noseDye);
        SetDye("Feet", _data.feetDye);
        SetDye("Horse_Pattern", _data.patternDye);
        SetPattern(_data.pattern);
        m_skinSelector.SetSkin(_data.skinData);
    }

    public override void ResetDye(string _colourSegment)
    {
        SetDye(_colourSegment, m_defaultMat.GetColor("_" + _colourSegment + "_Colour"), m_defaultMat.GetFloat("_" + _colourSegment + "_Metal"), m_defaultMat.GetFloat("_" + _colourSegment + "_Rough"));
    }

    public override Material GetMat(bool _previewMat) { return m_currentMat; }
}
