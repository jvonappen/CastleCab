using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public List<SO_Achievement> m_achievements;

    private void Awake()
    {
        foreach (SO_Achievement achievement in m_achievements)
        {
            if (achievement.AchievementType == AchievementType.Statistic)
            {

            }
        }
    }

    //private void Awake()
    //{
    //    GameStatistics.distanceTraveled.Changed += OnValueChanged;
    //}
    //
    //
    //private void Update()
    //{
    //    GameStatistics.distanceTraveled.Value++;
    //}
    //
    //void OnValueChanged(object _target, Observable<float>.ChangedEventArgs _args)
    //{
    //    Debug.Log("Distance Traveled = " + _args.NewValue);
    //}
}
