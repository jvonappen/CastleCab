using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    #region Singleton
    public static AchievementManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    #endregion

    public List<SO_Achievement> m_achievements;

    List<AchievementStatTracker> m_trackers = new();

    private void Start()
    {
        foreach (SO_Achievement achievement in m_achievements)
        {
            if (achievement.AchievementType == AchievementType.Statistic)
            {
                m_trackers.Add(new(achievement));
            }
        }
    }
}

public class AchievementStatTracker
{
    SO_Achievement m_data;

    public AchievementStatTracker(SO_Achievement _achievement)
    {
        m_data = _achievement;

        GameStatistics.GetStat(m_data.Statistic).Changed += OnStatChanged;
    }

    void OnStatChanged(object _obj, Observable<float>.ChangedEventArgs _args)
    {
        if (m_data.AmountForCompletion <= _args.NewValue) CompleteAchievement();
    }

    void CompleteAchievement()
    {
        GameStatistics.GetStat(m_data.Statistic).Changed -= OnStatChanged;
        Debug.Log("Completed Achievement: '" + m_data.DisplayName + "'");
    }
}
