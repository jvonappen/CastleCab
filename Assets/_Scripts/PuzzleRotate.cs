using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotate : MonoBehaviour
{
    private GameObject rotatingItem;

    [SerializeField] private float m_rotationValue;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private float m_waitTime;

    
    
    private bool m_rotateBack = false;
    private bool m_rotateForth = true;


    [SerializeField] private bool m_backAndForthLoop;
    [SerializeField] private bool m_continuousLoop;

    [SerializeField] private bool m_onHitBackAndForth;
    [SerializeField] private bool m_onHitContinuous;

    [Header("Debug")]
    [SerializeField] private float m_currentTransRot;
    [SerializeField] private float m_rotationReturnPoint;


    private void Awake()
    {
        rotatingItem = this.gameObject;
        m_rotationReturnPoint = transform.eulerAngles.y;

    }

    void Update()
    {
        m_currentTransRot = transform.eulerAngles.y;
        BackAndForthLoop();
    }

    private void BackAndForthLoop()
    {
        if (!m_backAndForthLoop) return;

        if(m_currentTransRot <= m_rotationValue && !m_rotateBack && m_rotateForth == true)
        {              
                transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0, Space.Self);
        }

        if(m_currentTransRot >= m_rotationValue && !m_rotateBack)
        {    
            m_rotateForth = false;
            TimerManager.RunAfterTime(() =>
            {
                m_rotateBack = true;
            }, m_waitTime);
        }

        if(m_rotateBack == true)
        {
                transform.Rotate(0, -m_rotationSpeed * Time.deltaTime, 0, Space.Self);
                Debug.Log("Returning to Point");          
        }
        if(m_rotateBack == true && m_currentTransRot <= m_rotationReturnPoint)
        {
            m_rotateBack = false;
            TimerManager.RunAfterTime(() =>
            {
                m_rotateForth = true;
            }, m_waitTime);
        }
        
    }

}
