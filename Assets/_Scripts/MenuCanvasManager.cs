using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject m_menu, m_blackoutCanvas;
    public void EnableMenu()
    {
        m_menu.SetActive(true);
        m_blackoutCanvas.SetActive(false);
    }
}
