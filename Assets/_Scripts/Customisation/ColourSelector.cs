using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSelector : MonoBehaviour
{
    SO_Dye m_mainDye, m_secondaryDye, m_tertiaryDye;
    public SO_Dye mainDye { get { return m_mainDye; } }
    public SO_Dye secondaryDye { get { return m_secondaryDye; } }
    public SO_Dye tertiaryDye { get { return m_tertiaryDye; } }

    ModelSelector m_modelSelector;
    private void Awake() => m_modelSelector = GetComponent<ModelSelector>();

    public void SetMainDye(SO_Dye _dye)
    {
        SetDye("Main", _dye);
        m_mainDye = _dye;
    }

    public void SetSecondaryDye(SO_Dye _dye)
    {
        SetDye("Secondary", _dye);
        m_secondaryDye = _dye;
    }

    public void SetTertiaryDye(SO_Dye _dye)
    {
        SetDye("Tertiary", _dye);
        m_tertiaryDye = _dye;
    }

    public void ResetMainDye() => ResetDye("Main");
    public void ResetSecondaryDye() => ResetDye("Secondary");
    public void ResetTertiaryDye() => ResetDye("Tertiary");

    void ResetDye(string _colourSegment)
    {
        ModelSettings settings = m_modelSelector.selectedObject.GetComponent<ModelSettings>();
        if (settings)
            SetDye(_colourSegment, settings.defaultMat.GetColor("_" + _colourSegment + "_Colour"), settings.defaultMat.GetFloat("_" + _colourSegment + "_Metal"), settings.defaultMat.GetFloat("_" + _colourSegment + "_Rough"));
    }

    void SetDye(string _colourSegment, SO_Dye _dye) => SetDye(_colourSegment, _dye.m_colour, _dye.m_metal, _dye.m_roughness);

    void SetDye(string _colourSegment, Color _colour, float _metal, float _roughness)
    {
        Material mat = m_modelSelector.GetMat();

        mat.SetColor("_" + _colourSegment + "_Colour", _colour);
        mat.SetFloat("_" + _colourSegment + "_Metal", _metal);
        mat.SetFloat("_" + _colourSegment + "_Rough", _roughness);
    }
}
