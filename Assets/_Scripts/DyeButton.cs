using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyeButton : MonoBehaviour
{
    ColourSelector m_selector;
    string m_dyeType;
    SO_Dye m_dye;

    public void Init(ColourSelector _colourSelector, string _type, SO_Dye _dye)
    {
        m_selector = _colourSelector;
        m_dyeType = _type;
        m_dye = _dye;

        GetComponent<Image>().color = _dye.m_colour;
    }

    public void SetDye() => m_selector.SetDye(m_dyeType, m_dye);
}
