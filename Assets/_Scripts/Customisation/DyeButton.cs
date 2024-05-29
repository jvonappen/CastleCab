using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyeButton : MonoBehaviour
{
    [SerializeField] Image m_colourDisplay;
    
    DyeCollection m_collection;
    SO_Dye m_dye;

    public void Init(DyeCollection _collection, SO_Dye _dye)
    {
        m_collection = _collection;
        m_dye = _dye;

        if (_dye) m_colourDisplay.color = _dye.m_colour;
    }

    public void OnSelect() => m_collection.OnSelect();

    public void SetDye() => m_collection.SelectDye(m_dye);
}
