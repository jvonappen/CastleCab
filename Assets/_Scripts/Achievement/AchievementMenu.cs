using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementMenu : MonoBehaviour
{
    [SerializeField] GameObject m_achievementDisplayPrefab;

    List<SO_Achievement> m_achievements;
    List<GameObject> m_achievementDisplays = new();
    private void Start()
    {
        m_achievements = AchievementManager.Instance.m_achievements;
        DisplayAchievements();
    }

    private void OnEnable()
    {
        m_achievements ??= AchievementManager.Instance.m_achievements;
        DisplayAchievements();
    }

    public void ClearAchievemetDisplays()
    {
        for (int i = m_achievementDisplays.Count - 1; i >= 0; i--) Destroy(m_achievementDisplays[i]);
        m_achievementDisplays.Clear();
    }

    public void DisplayAchievements()
    {
        ClearAchievemetDisplays();

        foreach (SO_Achievement achievement in m_achievements)
        {
            GameObject achievementDisplay = Instantiate(m_achievementDisplayPrefab, transform);
            m_achievementDisplays.Add(achievementDisplay);

            achievementDisplay.GetComponent<AchievementDisplay>().Display(achievement);
        }
    }
}
