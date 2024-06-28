using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoints : MonoBehaviour
{
    [SerializeField] List<Transform> m_remainingSpawnPoints;
    public List<Transform> remainingSpawnpoints { get { return m_remainingSpawnPoints; } set { m_remainingSpawnPoints = value; } }

    [SerializeField] bool m_randomiseSpawnpoint = false;
    public bool randomiseSpawnpoint { get { return m_randomiseSpawnpoint; } set { m_randomiseSpawnpoint = value; } }
}
