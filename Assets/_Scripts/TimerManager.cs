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

    public static Timer RunAfterTime(Action function, float time)
    {
        Timer timer = new(time, function);
        Instance.m_timers.Add(timer);
        return timer;
    }

    public static UpdateTimer RunUntilTime(Action<float, float> function, float time)
    {
        UpdateTimer timer = new(time, function);
        Instance.m_timers.Add(timer);
        return timer;
    }

    public static UpdateTimer RunUntilTime(Action function, float time)
    {
        UpdateTimer timer = new(time, function);
        Instance.m_timers.Add(timer);
        return timer;
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
    [SerializeField] protected float m_timeUntilEnd;
    [SerializeField] protected float m_counter = 0;

    Action m_functionToCall;

    public Timer(float time, Action function = null)
    {
        m_functionToCall = function;
        m_timeUntilEnd = time;
    }

    public void UpdateTimer()
    {
        if (m_counter >= m_timeUntilEnd)
        {
            if (m_functionToCall != null) RunFunction();
            TimerManager.DestroyTimer(this);
        }
        else
        {
            m_counter += Time.deltaTime;
            OnTimerUpdated();
        }
    }

    protected virtual void OnTimerUpdated() { }

    protected virtual void RunFunction()
    {
        try
        {
            m_functionToCall();
        }
        catch { Debug.LogWarning("TimerManager failed to invoke action. Script may no longer exist."); }
    }
}

[Serializable]
public class UpdateTimer : Timer
{
    Action<float, float> m_functionToCall;

    public UpdateTimer(float time, Action<float, float> function) : base(time)
    {
        m_functionToCall = function;
    }

    public UpdateTimer(float time, Action function) : base(time, function)
    {
        m_functionToCall = null;
    }

    protected override void OnTimerUpdated()
    {
        base.OnTimerUpdated();

        RunFunction();
    }

    protected override void RunFunction()
    {
        if (m_functionToCall == null) base.RunFunction();
        else
        {
            try
            {
                m_functionToCall(m_counter, m_timeUntilEnd);
            }
            catch { Debug.LogWarning("TimerManager failed to invoke action. Script may no longer exist."); }
        }
        
    }
}