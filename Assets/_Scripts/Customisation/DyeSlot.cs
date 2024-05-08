using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyeSlot : MonoBehaviour
{
    [SerializeField] DyeCollection m_collection;

    [SerializeField] ColourSelector m_selector;
    [SerializeField] string m_dyeType;

    Image m_buttonImage;
    Image m_colourSlot;

    Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(EquipDye);

        m_buttonImage = GetComponent<Image>();
        m_colourSlot = transform.GetChild(0).GetComponent<Image>();

        UpdateSlotColour();
    }

    private void OnEnable() => m_selector.modelSelector.onModelSelect += UpdateSlotColour;
    private void OnDisable() => m_selector.modelSelector.onModelSelect -= UpdateSlotColour;

    public void EquipDye()
    {
        m_selector.SetDye(m_dyeType, m_collection.selectedDye);
        UpdateSlotColour();
    }

    public void UpdateSlotColour()
    {
        Debug.Log("Updated");
        DyeData dye = m_selector.GetDye(m_dyeType);
        if (dye.colour != new Color())
        {
            m_buttonImage.color = Color.white;
            m_colourSlot.color = dye.colour;
        }
        else
        {
            m_buttonImage.color = new Color(255, 255, 255, 0.25f);
            m_colourSlot.color = new Color(255, 255, 255, 0.25f);
        }
    }

    public void OnSelect()
    {
        m_collection.categorySelector.SelectObject(transform.parent.gameObject);
    }
}
