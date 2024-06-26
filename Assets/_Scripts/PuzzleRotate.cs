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

    [SerializeField] private float m_rotationEndPoint;
    [SerializeField] private float m_rotationReturnPoint = 10;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] [ConditionalEnumHide("m_mode", 2)] private float m_waitTime;

    [Header("Debug")]
    [SerializeField] private bool m_rotateBack = false;
    [SerializeField] private bool m_rotateForth = true;

    [Header("Debug")]
    [SerializeField] private float m_currentTransRot;
    


    private void Awake()
    {
        //m_rotationReturnPoint = transform.eulerAngles.y;
    }

    void Update()
    {
        m_currentTransRot = transform.eulerAngles.y;

        BackAndForth();
        ContinuousRotation();
        ContinuousRotationWithStop();
    }

    private void BackAndForth()
    {
        if (m_mode != Mode.BackAndForthLoop) return;
        if(m_currentTransRot <= m_rotationEndPoint && !m_rotateBack && m_rotateForth == true) { transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self);}
        if(m_currentTransRot >= m_rotationEndPoint && !m_rotateBack)
        { m_rotateForth = false; m_rotateBack = false; TimerManager.RunAfterTime(() => { m_rotateBack = true; }, m_waitTime); }

        if(m_rotateBack == true && m_currentTransRot >= m_rotationReturnPoint)  { transform.Rotate(0, -m_rotationSpeed * Time.deltaTime, 0, Space.Self); }
        if(m_rotateBack == true && m_currentTransRot <= m_rotationReturnPoint)
        { m_rotateBack = false; m_rotateForth = false; TimerManager.RunAfterTime(() => { m_rotateForth = true; }, m_waitTime);}

    }

    private void ContinuousRotation()
    {
        if(m_mode != Mode.ContinuousLoop) return;
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self);

    }

    private void ContinuousRotationWithStop()
    {
        if (m_mode != Mode.ContinuousLoopWithStop) return;
      
        if (m_currentTransRot != m_rotationEndPoint && m_rotateForth) { transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self); }
        if(m_currentTransRot <= m_rotationEndPoint) { m_rotateBack = true; }
        if(m_currentTransRot >= m_rotationEndPoint && m_rotateBack) { m_rotateForth = false; m_rotateBack = false; TimerManager.RunAfterTime(() => { m_rotateForth = true; }, m_waitTime); }

    }
}
