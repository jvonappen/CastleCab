using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedStatisticTracker : StatisticTracker
{
    Vector3 m_lastPos;

    private void Awake() => m_lastPos = transform.position;
    private void Update()
    {
        m_valueToAdd = Vector3.Distance(transform.position, m_lastPos);
        m_lastPos = transform.position;

        UpdateStatistic();
    }
}
