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

    public void AchievementCompleted(SO_Achievement _achievement)
    {
        m_achievementNotifier = m_achievementNotifier != null ? m_achievementNotifier : FindObjectOfType<AchievementPopup>();
        if (m_achievementNotifier)
        {
            m_achievementNotifier.Display(_achievement.DisplayName, _achievement.Icon);
        }

        m_completedAchievements.Add(_achievement);
        Debug.Log("Completed Achievement: '" + _achievement.DisplayName + "'");
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
        AchievementManager.Instance.AchievementCompleted(m_data);
    }
}
