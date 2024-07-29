using System.Collections.Generic;
using System.Linq;
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

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    #endregion

    AchievementPopup m_achievementNotifier;
    
    public List<SO_Achievement> m_achievements;

    List<AchievementStatTracker> m_statTrackers = new();

    List<SO_Achievement> m_completedAchievements = new();
    public bool IsAchievementCompleted(SO_Achievement _achievement) => m_completedAchievements.Contains(_achievement);
    public float GetAchievementProgress(SO_Achievement _achievement)
    {
        AchievementStatTracker tracker = m_statTrackers.FirstOrDefault(t => t.data == _achievement);
        if (tracker != null) return tracker.lastVal;
        else return 0;
    }

    private void Start()
    {
        foreach (SO_Achievement achievement in m_achievements)
        {
            if (achievement.AchievementType == AchievementType.Statistic)
            {
                m_statTrackers.Add(new(achievement));
            }
        }
    }

    public void CompleteAchievement(SO_Achievement _achievement)
    {
        m_achievementNotifier = m_achievementNotifier != null ? m_achievementNotifier : FindObjectOfType<AchievementPopup>();
        if (m_achievementNotifier)
        {
            m_achievementNotifier.Display(_achievement.DisplayName, _achievement.Icon);
        }

        m_completedAchievements.Add(_achievement);
    }
}

public class AchievementStatTracker
{
    public SO_Achievement data { get; private set; }
    public float lastVal { get; private set; }

    public AchievementStatTracker(SO_Achievement _achievement)
    {
        data = _achievement;

        GameStatistics.GetStat(data.Statistic).Changed += OnStatChanged;
    }

    void OnStatChanged(object _obj, Observable<float>.ChangedEventArgs _args)
    {
        lastVal = _args.NewValue;
        if (data.AmountForCompletion <= _args.NewValue) CompleteAchievement();
    }

    void CompleteAchievement()
    {
        GameStatistics.GetStat(data.Statistic).Changed -= OnStatChanged;
        AchievementManager.Instance.CompleteAchievement(data);
    }
}
