using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TrackInfo
{
    [SerializeField] float m_valueToAdd;
    [SerializeField] Statistic m_statistic;

    public float ValueToAdd { get { return m_valueToAdd; } }
    public Statistic Statistic { get { return m_statistic; } }
}

public class StatisticTracker : MonoBehaviour
{
    [SerializeField] List<TrackInfo> m_statisticsToTrack;

    /// <summary>
    /// Adds 'ValueToAdd' of each statistic in list to its respective value in GameStatistics. 
    /// </summary>
    public virtual void UpdateStatistic()
    {
        foreach (TrackInfo trackInfo in m_statisticsToTrack)
        {
            GameStatistics.GetStat(trackInfo.Statistic).Value += GetValueToAdd(trackInfo);
        }
    }

    public virtual float GetValueToAdd(TrackInfo _trackInfo) => _trackInfo.ValueToAdd;
}
