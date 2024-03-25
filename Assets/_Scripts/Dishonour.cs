using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishonour : MonoBehaviour
{
    [SerializeField] PointProgress m_progress;

    [SerializeField] SO_DishonourData m_data;

    int m_maxDishonourLevel = 0;

    [Header("Current")]
    [SerializeField] int m_currentDishonourPoints = 0;

    [Tooltip("Max value based off PointProgress max, update the progress value if the max value isn't updating")] 
    [SerializeField] int m_currentDishonourLevel = 0;

    private void Start()
    {
        m_maxDishonourLevel = m_progress.totalPoints;
    }

    public void AddDishonour(int _dishonourToAdd)
    {
        m_currentDishonourPoints += _dishonourToAdd;
        UpdateDishonourProgress();
    }

    private void OnValidate()
    {
        if (m_progress)
        {
            m_progress.OnValidate();
            m_maxDishonourLevel = m_progress.totalPoints;

            if (m_currentDishonourLevel < 0) m_currentDishonourLevel = 0;
            else if (m_currentDishonourLevel > m_maxDishonourLevel) m_currentDishonourLevel = m_maxDishonourLevel;

            if (m_data) UpdateDishonourProgress();
        }
        else Debug.LogWarning("Add a PointProgress reference for script to function");
    }

    void UpdateDishonourProgress()
    {
        if (m_currentDishonourPoints >= m_data.m_dishonourPointsPerLevel)
        {
            if (m_currentDishonourLevel < m_maxDishonourLevel)
            {
                m_currentDishonourPoints -= m_data.m_dishonourPointsPerLevel;
                m_currentDishonourLevel++;

                UpdateDishonourProgress();
                return;
            }
        }
        else if (m_currentDishonourPoints < 0)
        {
            if (m_currentDishonourLevel > 0)
            {
                m_currentDishonourPoints += m_data.m_dishonourPointsPerLevel;
                m_currentDishonourLevel--;

                UpdateDishonourProgress();
                return;
            }
            else m_currentDishonourPoints = 0;
        }

        if (m_currentDishonourLevel == m_maxDishonourLevel) m_currentDishonourPoints = 0;

        m_progress.SetProgress(m_currentDishonourLevel);
    }
}
