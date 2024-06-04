using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveArrow : MonoBehaviour
{
    [SerializeField] float m_distanceFromPlayer = 175;
    [SerializeField] Transform m_horse;

    [SerializeField] RectTransform m_canvas;

    [SerializeField] RectTransform m_rotatePoint;
    [SerializeField] Transform m_tempTarget;

    float m_distance;

    private void Update()
    {
        Vector3 dir = m_tempTarget.position - m_horse.position;
        //m_distance = dir.magnitude;

        Vector3 localDir = m_horse.worldToLocalMatrix.MultiplyVector(dir);
        Vector2 screenDir = new(localDir.x, localDir.z);
        screenDir.Normalize();

        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = m_rotatePoint.anchoredPosition + (screenDir * m_distanceFromPlayer);

        rt.up = -screenDir;
        rt.localEulerAngles = new(0, 0, rt.localEulerAngles.z); 
        //rt.LookAt(m_rotatePoint);
        //Debug.Log(Vector3.Cross(m_rotatePoint.forward, -screenDir).y);
    }
}
