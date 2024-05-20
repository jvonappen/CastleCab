using UnityEngine;

public class MultiColourSelector : ColourSelector
{
    ModelSelector m_modelSelector;
    public ModelSelector modelSelector { get { return m_modelSelector; } }
    private void Awake() => m_modelSelector = GetComponent<ModelSelector>();

    public void SetMainDye(SO_Dye _dye) => SetDye("Main", _dye);
    public void SetSecondaryDye(SO_Dye _dye) => SetDye("Secondary", _dye);
    public void SetTertiaryDye(SO_Dye _dye) => SetDye("Tertiary", _dye);

    public void ResetMainDye() => ResetDye("Main");
    public void ResetSecondaryDye() => ResetDye("Secondary");
    public void ResetTertiaryDye() => ResetDye("Tertiary");

    public override void ResetDye(string _colourSegment)
    {
        if (m_modelSelector.previewObject)
        {
            ModelSettings settings = m_modelSelector.previewObject.GetComponent<ModelSettings>();
            if (settings)
                SetDye(_colourSegment, settings.defaultMat.GetColor("_" + _colourSegment + "_Colour"), settings.defaultMat.GetFloat("_" + _colourSegment + "_Metal"), settings.defaultMat.GetFloat("_" + _colourSegment + "_Rough"));
        }
    }

    public override Material GetMat()
    {
        if (!m_modelSelector) m_modelSelector = GetComponent<ModelSelector>();
        return m_modelSelector.GetMat();
    }
}
