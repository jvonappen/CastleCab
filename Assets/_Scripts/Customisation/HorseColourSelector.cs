using UnityEngine;

public class HorseColourSelector : ColourSelector
{
    Material m_defaultMat, m_currentMat;
    private void Awake()
    {
        m_defaultMat = GetComponent<Renderer>().sharedMaterial;

        // Instances material
        m_currentMat = new(m_defaultMat); 
        GetComponent<Renderer>().sharedMaterial = m_currentMat;
    }

    public void SetBaseDye(SO_Dye _dye) => SetDye("Base", _dye);
    public void SetHairDye(SO_Dye _dye) => SetDye("Hair", _dye);
    public void SetTailDye(SO_Dye _dye) => SetDye("Tail", _dye);
    public void SetNoseDye(SO_Dye _dye) => SetDye("Nose", _dye);
    public void SetFeetDye(SO_Dye _dye) => SetDye("Feet", _dye);
    public void SetPatternDye(SO_Dye _dye) => SetDye("Horse_Pattern", _dye);

    public void ResetBaseDye() => ResetDye("Base");
    public void ResetHairDye() => ResetDye("Hair");
    public void ResetTailDye() => ResetDye("Tail");
    public void ResetNoseDye() => ResetDye("Nose");
    public void ResetFeetDye() => ResetDye("Feet");
    public void ResetPatternDye() => ResetDye("Horse_Pattern");

    public void SetDyes(HorseMatInformation _data)
    {
        SetDye("Base", _data.baseDye);
        SetDye("Hair", _data.hairDye);
        SetDye("Tail", _data.tailDye);
        SetDye("Nose", _data.noseDye);
        SetDye("Feet", _data.feetDye);
        SetDye("Horse_Pattern", _data.patternDye);
    }

    public override void ResetDye(string _colourSegment)
    {
        SetDye(_colourSegment, m_defaultMat.GetColor("_" + _colourSegment + "_Colour"), m_defaultMat.GetFloat("_" + _colourSegment + "_Metal"), m_defaultMat.GetFloat("_" + _colourSegment + "_Rough"));
    }

    public override Material GetMat() { return m_currentMat; }
}
