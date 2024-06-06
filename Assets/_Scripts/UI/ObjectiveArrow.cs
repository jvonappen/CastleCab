using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveArrow : MonoBehaviour
{
    [SerializeField] float m_distanceFromPlayer = 175;
    [SerializeField] Transform m_horse;

    [SerializeField] RectTransform m_rotatePoint;
    [SerializeField] Transform m_target;
    public void SetTarget(Transform _target) => m_target = _target;

    Image m_image;

    float m_distance;

    private void Awake() => m_image = GetComponent<Image>();

    private void Update()
    {
        if (m_target)
        {
            m_image.enabled = true;

            Vector3 dir = m_target.position - m_horse.position;
            //m_distance = dir.magnitude;

            Vector3 localDir = m_horse.worldToLocalMatrix.MultiplyVector(dir);
            Vector2 screenDir = new(localDir.x, localDir.z);
            screenDir.Normalize();

            RectTransform rt = GetComponent<RectTransform>();
            rt.anchoredPosition = m_rotatePoint.anchoredPosition + (screenDir * m_distanceFromPlayer);

            rt.up = -screenDir;
            rt.localEulerAngles = new(0, 0, rt.localEulerAngles.z);
        }
        else m_image.enabled = false;
    }
}
