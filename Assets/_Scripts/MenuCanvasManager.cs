using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject m_menu;
    public void EnableMenu() => m_menu.SetActive(true);
}
