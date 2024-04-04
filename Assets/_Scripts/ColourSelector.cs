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

    void SetDye(string _colourSegment, SO_Dye _dye)
    {
        Material mat = m_modelSelector.GetMat();

        mat.SetColor("_" + _colourSegment + "_Colour", _dye.m_colour);
        mat.SetFloat("_" + _colourSegment + "_Metal", _dye.m_metal);
        mat.SetFloat("_" + _colourSegment + "_Rough", _dye.m_roughness);
    }
}
