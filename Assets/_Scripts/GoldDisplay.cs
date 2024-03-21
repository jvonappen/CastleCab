using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GoldDisplay : MonoBehaviour
{
    [SerializeField] string m_displayTextPrefix = "Gold: ";

    GameManager m_manager;
    TextMeshProUGUI m_display;

    void Start()
    {
        m_manager = GameManager.Instance;
        m_display = GetComponent<TextMeshProUGUI>();

        if (m_manager) m_manager.onGoldChanged += UpdateDisplay;
        else Debug.Log("GameManager is being referenced but it does not exist");
    }

    void UpdateDisplay(int _oldVal, int _newVal) => UpdateDisplay();
    public void UpdateDisplay() => m_display.text = "Gold: " + m_manager.gold.ToString();
}
