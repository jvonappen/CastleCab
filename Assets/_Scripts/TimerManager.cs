using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    #region Singleton

    private static TimerManager m_instance;

    public static TimerManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("TimerManager").AddComponent<TimerManager>();

                DontDestroyOnLoad(m_instance.gameObject);
            }

            return m_instance;
        }
    }

    #endregion

    [SerializeField] List<Timer> m_timers = new();

    public static void RunAfterTime(Action function, float time)
    {
        Instance.m_timers.Add(new Timer(function, time));
    }

    public static void DestroyTimer(Timer timer)
    {
        Instance.m_timers.Remove(timer);
    }

    private void Update()
    {
        for (int i = 0; i < m_timers.Count; i++)
        {
            m_timers[i].UpdateTimer();
        }
    }
}

[Serializable]
public class Timer
{
    [SerializeField] float m_timeUntilEnd;
    [SerializeField] float m_counter = 0;

    Action m_functionToCall;

    public Timer(Action function, float time)
    {
        m_functionToCall = function;
        m_timeUntilEnd = time;
    }

    public void UpdateTimer()
    {
        if (m_counter >= m_timeUntilEnd)
        {
            try
            {
                m_functionToCall();
            }
            catch { Debug.LogWarning("TimerManager failed to invoke action. Script may no longer exist."); }
            TimerManager.DestroyTimer(this);
        }
        else m_counter += Time.deltaTime;
    }
}