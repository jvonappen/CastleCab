using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedStatisticTracker : StatisticTracker
{
    Vector3 m_lastPos;
    float m_movedDist;

    private void Awake()
    {
        m_lastPos = transform.position;
    }

    private void Update()
    {
        m_movedDist = Vector3.Distance(transform.position, m_lastPos);
        m_lastPos = transform.position;

        UpdateStatistic();
    }

    public override float GetValueToAdd(TrackInfo _trackInfo) => m_movedDist;
}
