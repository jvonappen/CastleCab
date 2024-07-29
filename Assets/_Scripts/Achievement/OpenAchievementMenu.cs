using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenAchievementMenu : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_playerInput;
    GameObject m_achievementMenu;
    private void Start()
    {
        m_achievementMenu = FindObjectOfType<AchievementMenu>(true).transform.parent/*.parent.parent.parent*/.gameObject;

        m_playerInput.m_playerControls.Controls.StatsMenu.performed += ToggleMenu;
    }
    void ToggleMenu(InputAction.CallbackContext context) => m_achievementMenu.SetActive(!m_achievementMenu.activeSelf);
}
