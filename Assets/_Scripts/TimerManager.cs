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

    public static void RunAfterTime(Action function, float time) => Instance.m_timers.Add(new Timer(function, time));

    public static void RunUntilTime(Action function, float time) => Instance.m_timers.Add(new Timer(function, time, true));

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

    bool m_functionOnUpdate;

    public Timer(Action function, float time, bool functionOnUpdate = false)
    {
        m_functionToCall = function;
        m_timeUntilEnd = time;
        m_functionOnUpdate = functionOnUpdate;
    }

    public void UpdateTimer()
    {
        if (m_counter >= m_timeUntilEnd)
        {
            RunFunction();
            TimerManager.DestroyTimer(this);
        }
        else
        {
            m_counter += Time.deltaTime;
            if (m_functionOnUpdate) RunFunction();
        }
    }

    void RunFunction()
    {
        try
        {
            m_functionToCall();
        }
        catch { Debug.LogWarning("TimerManager failed to invoke action. Script may no longer exist."); }
    }
}