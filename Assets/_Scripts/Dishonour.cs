using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishonour : MonoBehaviour
{
    [SerializeField] PointProgress m_progress;

    int m_maxDishonourLevel = 0;
    [SerializeField] int m_dishonourPointsPerLevel = 100;

    [Header("Current")]
    [SerializeField] int m_currentDishonourPoints = 0;

    [Tooltip("Max value based off PointProgress max, update the progress value if the max value isn't updating")] 
    [SerializeField] int m_currentDishonourLevel = 0;
    public int currentDishonour { get { return m_currentDishonourLevel; } internal set { m_currentDishonourLevel = value; OnDishonourChanged(); } }

    public Action onDishonourChanged;

    EnemySpawner m_enemySpawner;

    private void Start()
    {
        m_enemySpawner = FindObjectOfType<EnemySpawner>();

       m_maxDishonourLevel = m_progress.totalPoints;
    }

    public void AddDishonour(int _dishonourToAdd)
    {
        m_currentDishonourPoints += _dishonourToAdd;
        UpdateDishonourProgress();
    }

    public void DecreaseDishonour(int _dishonourToAdd)
    {
        m_currentDishonourPoints -= _dishonourToAdd;
        UpdateDishonourProgress();
    }

    void OnDishonourChanged()
    {
        if (m_enemySpawner) m_enemySpawner.UpdateEnemies();

        onDishonourChanged?.Invoke();
    }

    private void OnValidate()
    {
        if (m_progress)
        {
           // m_progress.OnValidate();
            //m_maxDishonourLevel = m_progress.totalPoints;

            if (m_currentDishonourLevel < 0) currentDishonour = 0;
            else if (m_currentDishonourLevel > m_maxDishonourLevel) currentDishonour = m_maxDishonourLevel;

            UpdateDishonourProgress();
        }
        else Debug.LogWarning("Add a PointProgress reference for script to function");
    }

    void UpdateDishonourProgress()
    {
        if (m_currentDishonourPoints >= m_dishonourPointsPerLevel)
        {
            if (m_currentDishonourLevel < m_maxDishonourLevel)
            {
                m_currentDishonourPoints -= m_dishonourPointsPerLevel;
                currentDishonour++;

                UpdateDishonourProgress();
                return;
            }
        }
        else if (m_currentDishonourPoints < 0)
        {
            if (m_currentDishonourLevel > 0)
            {
                m_currentDishonourPoints += m_dishonourPointsPerLevel;
                currentDishonour--;

                UpdateDishonourProgress();
                return;
            }
            else m_currentDishonourPoints = 0;
        }

        if (m_currentDishonourLevel == m_maxDishonourLevel) m_currentDishonourPoints = 0;

        m_progress.SetProgress(m_currentDishonourLevel);
    }
}
