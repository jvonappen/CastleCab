using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSelector : MonoBehaviour
{
    ModelSelector m_modelSelector;

    private void Awake()
    {
        m_modelSelector = GetComponent<ModelSelector>();
    }

    Material GetMat() => m_modelSelector.selectedObject.GetComponent<Renderer>().sharedMaterial;

    public void SetMainDye(SO_Dye _dye)
    {
        Material mat = GetMat();

        Debug.Log(mat);
        Debug.Log("Setting colour to " + _dye);

        
        mat.SetColor("_Main_Colour", _dye.m_colour);
        mat.SetFloat("_Main_Metal", _dye.m_metal);
        mat.SetFloat("_Main_Rough", _dye.m_roughness);
    }

    //public void SetSecondaryDye(Color _colour)
    //{
    //    GetMat().SetColor("Secondary_Colour", _colour);
    //}
    //
    //public void SetTertiaryDye(Color _colour)
    //{
    //    GetMat().SetColor("Tertiary_Colour", _colour);
    //}
}
