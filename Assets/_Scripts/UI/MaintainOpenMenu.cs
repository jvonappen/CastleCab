using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainOpenMenu : MonoBehaviour
{
    [SerializeField] ChangeMenu m_activeOnAwake;
    [SerializeField] List<ObjectTwin> m_menus;

    //private void Awake() => OpenMenu();

    public void OpenMenu()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(false);
        if (m_activeOnAwake) m_activeOnAwake.SetMenu();
    }

    public void SwitchMenus()
    {
        foreach (ObjectTwin menu in m_menus)
        {
            if (menu.gameObject.activeSelf)
            {
                menu.m_twin.SetActive(true);

                menu.gameObject.SetActive(false);
            }
        }
    }
}
