using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using URNTS;

[RequireComponent(typeof(TrafficManager))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SO_EnemyData m_data;

    Dictionary<int, List<GameObject>> m_enemies;

    TrafficManager m_trafficManager;
    GameManager m_manager;

    private void Start()
    {
        m_trafficManager = GetComponent<TrafficManager>();
        m_manager = GameManager.Instance;

        m_enemies = new Dictionary<int, List<GameObject>>();
        UpdateEnemies();
    }

    public void UpdateEnemies()
    {
        // Update every enemy type
        foreach (EnemyInfo enemyInfo in m_data.m_enemies) UpdateEnemy(enemyInfo);
    }

    public void UpdateEnemy(EnemyInfo _enemyInfo)
    {
        int spawnAmount = GetEnemySpawnAmount(_enemyInfo);

        // If enemy type doesn't exist in dictionary already, add it
        if (!m_enemies.ContainsKey(_enemyInfo.starAmount)) m_enemies.Add(_enemyInfo.starAmount, new());

        List<GameObject> enemyList = m_enemies[_enemyInfo.starAmount];

        if (enemyList.Count < spawnAmount) // Add enemy
        {
            int amountToSpawn = spawnAmount - enemyList.Count;
            //Debug.Log("Enemy type '" + _enemyInfo.starAmount + "' needs to spawn '" + amountToSpawn + "' enemies");

            for (int i = 0; i < amountToSpawn; i++)
            {
                NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
                int vertexIndex = Random.Range(0, triangulation.vertices.Length);
                //Debug.Log(triangulation.vertices[vertexIndex]);
                //NavMeshHit hit;
                //if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, 0))
                //{
                //    GameObject enemyToSpawn = Instantiate(_enemyInfo.m_enemyPrefab);
                //    enemyList.Add(enemyToSpawn);
                //}
                //else { Debug.Log("Huh?"); }

                GameObject enemyToSpawn = Instantiate(_enemyInfo.m_enemyPrefab, triangulation.vertices[vertexIndex], Quaternion.identity);
                enemyList.Add(enemyToSpawn);
            }
        }
        else if (enemyList.Count > spawnAmount) // Remove enemy
        {
            int amountToDespawn = enemyList.Count - spawnAmount;
            //Debug.Log("Enemy type '" + _enemyInfo.starAmount + "' needs to despawn '" + amountToDespawn + "' enemies");

            for (int i = 0; i < amountToDespawn; i++)
            {
                Destroy(enemyList[0]);
                enemyList.RemoveAt(0);
            }
        }

        //Debug.Log("Enemy type '" + _enemyInfo.starAmount + "' spawned '" + spawnAmount + "' enemies"); - Total enemies
    }

    #region GetValues
    public int GetEnemySpawnAmount(EnemyInfo _enemyInfo)
    {
        int spawnAmount = 0;

        // Calculates spawn amount for enemy based off amount of players over minimum dishonour level
        foreach (GameObject player in m_manager.players)
        {
            Dishonour playerDishonour = player.GetComponent<Dishonour>();
            if (!playerDishonour) break;

            // Only increases spawn amount if player has enough dishonour
            if (playerDishonour.currentDishonour >= _enemyInfo.starAmount) spawnAmount += _enemyInfo.enemiesPerPlayer;
        }

        // If there are no enemies of this type spawning,
        // check if the enemy is supposed to spawn passive, if so make some spawn still
        if (spawnAmount == 0)
        {
            if (!_enemyInfo.onlySpawnIfDishonoured) spawnAmount = _enemyInfo.enemiesPerPlayer;
        }

        return spawnAmount;
    }
    #endregion
}
