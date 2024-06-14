using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotate : MonoBehaviour
{
    public enum Mode
    {
        BackAndForthLoop,
        ContinuousLoop,
        ContinuousLoopWithStop,
        InteractBackAndForth,
        InteractContinuous
    }
    [SerializeField] Mode m_mode;

    [SerializeField] private float m_rotationPoint;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] [ConditionalEnumHide("m_mode", 2)] private float m_waitTime;


    private bool m_rotateBack = false;
    private bool m_rotateForth = true;

    [Header("Debug")]
    [SerializeField] private float m_currentTransRot;
    [SerializeField] private float m_rotationReturnPoint;


    private void Awake()
    {
        m_rotationReturnPoint = transform.eulerAngles.y;
    }

    void Update()
    {
        m_currentTransRot = transform.eulerAngles.y;

        BackAndForthLoop();
        ContinuousLoop();
        ContinuousLoopWithStop();
    }

    private void BackAndForthLoop()
    {
        if (m_mode != Mode.BackAndForthLoop) return;
        if(m_currentTransRot <= m_rotationPoint && !m_rotateBack && m_rotateForth == true) { transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self);}
        if(m_currentTransRot >= m_rotationPoint && !m_rotateBack)
        { m_rotateForth = false; TimerManager.RunAfterTime(() => { m_rotateBack = true; }, m_waitTime); }

        if(m_rotateBack == true)  { transform.Rotate(0, -m_rotationSpeed * Time.deltaTime, 0, Space.Self); }
        if(m_rotateBack == true && m_currentTransRot <= m_rotationReturnPoint)
        { m_rotateBack = false; TimerManager.RunAfterTime(() => { m_rotateForth = true; }, m_waitTime);}        
    }

    private void ContinuousLoop()
    {
        if(m_mode != Mode.ContinuousLoop) return;
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    private void ContinuousLoopWithStop()
    {
        if (m_mode != Mode.ContinuousLoopWithStop) return;
        if (m_currentTransRot != m_rotationPoint && m_rotateForth) { transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self); }
        if(m_currentTransRot <= m_rotationPoint) { m_rotateBack = true; }
        if(m_currentTransRot >= m_rotationPoint && m_rotateBack) { m_rotateForth = false; m_rotateBack = false; TimerManager.RunAfterTime(() => { m_rotateForth = true; }, m_waitTime); }
    }
}
