using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [Header("Debug-Testing")]
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_villager;

    [SerializeField] private float X;
    [SerializeField] private float Y;
    [SerializeField] private float Z;

    

    private void Awake()
    {
        m_villager = this.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"  || other.tag == "Wagon")
        {
            m_player = other.gameObject;
        }
    }

    void LateUpdate()
    {
        if(m_player!= null) 
        {
            transform.LookAt(m_player.transform);
            transform.Rotate(X, Y, Z);
        }
        
    }
}
