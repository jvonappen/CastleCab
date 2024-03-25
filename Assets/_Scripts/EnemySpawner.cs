using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URNTS;

[RequireComponent(typeof(TrafficManager))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SO_EnemyData m_data;

    TrafficManager m_trafficManager;

    private void Awake()
    {
        m_trafficManager = GetComponent<TrafficManager>();
    }


}
