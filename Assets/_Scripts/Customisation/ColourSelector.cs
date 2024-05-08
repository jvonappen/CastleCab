using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DyeData
{
    public Color colour;
    public float metal;
    public float roughness;
}

public class ColourSelector : MonoBehaviour
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

    void ResetDye(string _colourSegment)
    {
        if (m_modelSelector.previewObject)
        {
            ModelSettings settings = m_modelSelector.previewObject.GetComponent<ModelSettings>();
            if (settings)
                SetDye(_colourSegment, settings.defaultMat.GetColor("_" + _colourSegment + "_Colour"), settings.defaultMat.GetFloat("_" + _colourSegment + "_Metal"), settings.defaultMat.GetFloat("_" + _colourSegment + "_Rough"));
        }
    }

    public void SetDye(string _colourSegment, SO_Dye _dye)
    {
        if (_dye) SetDye(_colourSegment, _dye.m_colour, _dye.m_metal, _dye.m_roughness);
        else ResetDye(_colourSegment);
    }
    public void SetDye(string _colourSegment, DyeData _dye) => SetDye(_colourSegment, _dye.colour, _dye.metal, _dye.roughness);

    void SetDye(string _colourSegment, Color _colour, float _metal, float _roughness)
    {
        Material mat = m_modelSelector.GetMat();
        if (mat)
        {
            mat.SetColor("_" + _colourSegment + "_Colour", _colour);
            mat.SetFloat("_" + _colourSegment + "_Metal", _metal);
            mat.SetFloat("_" + _colourSegment + "_Rough", _roughness);
        }
    }

    public DyeData GetDye(string _colourSegment)
    {
        Material mat = m_modelSelector.GetMat();
        if (mat)
        {
            DyeData data = new();

            data.colour = mat.GetColor("_" + _colourSegment + "_Colour");
            data.metal = mat.GetFloat("_" + _colourSegment + "_Metal");
            data.roughness = mat.GetFloat("_" + _colourSegment + "_Rough");

            return data;
        }

        return new();
    }
}
