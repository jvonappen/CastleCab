using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSelector : MonoBehaviour
{
    ModelSelector m_modelSelector;
    private void Awake() => m_modelSelector = GetComponent<ModelSelector>();

    public void SetMainDye(SO_Dye _dye) => SetDye("Main", _dye);

    public void SetSecondaryDye(SO_Dye _dye) => SetDye("Secondary", _dye);

    public void SetTertiaryDye(SO_Dye _dye) => SetDye("Tertiary", _dye);

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
