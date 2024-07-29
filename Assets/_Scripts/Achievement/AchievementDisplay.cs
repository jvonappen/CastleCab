using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] GameObject m_completed, m_incomplete;
    [SerializeField] TextMeshProUGUI m_achievementName, m_achievementDescription;

    [SerializeField] Color m_completedTextColour, m_incompleteTextColour;

    public void Display(SO_Achievement _achievement)
    {
        bool isCompleted = AchievementManager.Instance.IsAchievementCompleted(_achievement);
        m_completed.SetActive(isCompleted);
        m_incomplete.SetActive(!isCompleted);

        if (isCompleted) m_achievementName.color = m_completedTextColour;
        else
        {
            m_achievementName.color = m_incompleteTextColour;

            if (_achievement.AchievementType == AchievementType.Statistic)
            {
                TextMeshProUGUI progressText = m_incomplete.GetComponentInChildren<TextMeshProUGUI>();

                float conversionMulti = _achievement.AmountForCompletion >= 1000 ? 0.001f : 1;

                float progress = AchievementManager.Instance.GetAchievementProgress(_achievement) * conversionMulti;
                progress = Mathf.Round(progress * 10) * 0.1f;

                progressText.text = progress.ToString() + "/" + _achievement.AmountForCompletion * conversionMulti;
            }
        }

        m_achievementName.text = _achievement.DisplayName;
        m_achievementDescription.text = _achievement.Description;
    }
}
