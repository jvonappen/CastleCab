using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawning
{
    [SerializeField] int m_starAmount;

    [Space(10)]
    [SerializeField] GameObject m_enemyPrefab;
    [SerializeField] int m_enemiesPerPlayer;
    [SerializeField] bool m_onlySpawnIfDishonoured;
    
    public int starAmount { get { return m_starAmount; } }
    public bool onlySpawnIfDishonoured { get { return m_onlySpawnIfDishonoured; } }
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
public class SO_EnemyData : ScriptableObject
{
    public List<EnemySpawning> m_enemySpawning;
}
