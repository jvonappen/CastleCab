using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticTracker : MonoBehaviour
{
    [SerializeField] Statistic m_statisticToTrack;
    [ConditionalEnumHide("m_statisticToTrack", 1, Enum1Inverse = true)] [SerializeField] protected float m_valueToAdd = 1;

    /// <summary>
    /// Adds 'm_valueToAdd' of statistic in GameStatistics. 
    /// </summary>
    public virtual void UpdateStatistic()
    {
        GameStatistics.GetStat(m_statisticToTrack).Value += m_valueToAdd;
    }
}
