using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyeMenu : MonoBehaviour
{
    [SerializeField] CustomisationDisplay m_customisationDisplay;
    [SerializeField] List<DyeSlot> m_dyeSlots;

    public void SetDyeSelector(ColourSelector _colourSelector)
    {
        foreach (DyeSlot dyeSlot in m_dyeSlots)
        {
            dyeSlot.SetColourSelector(_colourSelector);
            dyeSlot.UpdateSlotColour();
        }
    }

    public void OpenMenu(ColourSelector _colourSelector)
    {
        m_customisationDisplay.SetMenu(gameObject);
        SetDyeSelector(_colourSelector);
    }
}
