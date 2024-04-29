using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainOpenMenu : MonoBehaviour
{
    [SerializeField] List<ObjectTwin> m_menus;

    private void Awake()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(false);
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
