using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] GameObject m_completed, m_incomplete;
    [SerializeField] TextMeshProUGUI m_achievementName, m_achievementDescription;

    public void Display(SO_Achievement _achievement)
    {
        bool isCompleted = AchievementManager.Instance.IsAchievementCompleted(_achievement);
        m_completed.SetActive(isCompleted);
        m_incomplete.SetActive(!isCompleted);

        m_achievementName.text = _achievement.DisplayName;
        m_achievementDescription.text = _achievement.Description;
    }
}
