using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSpin : MonoBehaviour
{
    [SerializeField] float m_spinSpeed = 1;
    [SerializeField] Space m_relativeTo;

    private void Update() => transform.Rotate(Vector3.up, Time.deltaTime * m_spinSpeed, m_relativeTo);
}
