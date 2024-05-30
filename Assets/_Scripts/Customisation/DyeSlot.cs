using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DyeSlot : MonoBehaviour
{
    [SerializeField] DyeCollection m_collection;

    [SerializeField] bool m_selectNextSlot;

    public CustomButton m_nextSlot;
    public GameObject m_buttonToSelect;

    ColourSelector m_selector;
    public void SetColourSelector(ColourSelector _selector) => m_selector = _selector;

    [SerializeField] string m_dyeType;

    Image m_buttonImage;
    Image m_colourSlot;

    Button m_button;

    [SerializeField] UnityEvent m_onDye;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);

        m_buttonImage = GetComponent<Image>();
        m_colourSlot = transform.GetChild(0).GetComponent<Image>();

        if (m_selector == null) Debug.LogWarning("colour selector not set for dye slot");
    }

    private void Start() => UpdateSlotColour();

    public void SetDyeType() => m_collection.SetSlot(this);

    public void SetDye(SO_Dye _dye)
    {
        m_selector.SetDye(m_dyeType, _dye);
        UpdateSlotColour();

        m_onDye?.Invoke();
    }

    public void OnClick()
    {
        m_collection.OnSlotClicked(this);
    }

    public void SelectNextSlot()
    {
        GetComponent<CustomButton>().Deselect();
        if (!m_selectNextSlot) m_nextSlot.Select();
        else m_collection.m_eventSystem.SetSelectedGameObject(m_nextSlot.gameObject);
    }

    public void UpdateSlotColour()
    {
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
}
