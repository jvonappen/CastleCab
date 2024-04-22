using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainOpenMenu : MonoBehaviour
{
    [SerializeField] List<ObjectTwin> m_menus;

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
