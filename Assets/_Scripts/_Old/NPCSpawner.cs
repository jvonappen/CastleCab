using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] npcPrefabs;
    [SerializeField] Transform respawnPoint;
    [SerializeField] float respawnTime;
    [SerializeField] int maxNPCs = 40;
    public List<GameObject> npcPool = new List<GameObject>();

    private float timer = 0f;

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if enough time has passed to spawn a new NPC
        if (timer >= respawnTime)
        {
            // Reset the timer
            timer = 0f;

            // Check if the maximum limit is reached
            if (npcPool != null && npcPool.Count < maxNPCs)
            {
                // Spawn a random NPC prefab at the respawn point
                GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
                GameObject newNPC = Instantiate(npcPrefab, respawnPoint.position, respawnPoint.rotation);
                // Add the spawned NPC to the pool
                npcPool.Add(newNPC);

            }
            else
            {
                // Remove null references from npcPool
                npcPool.RemoveAll(npc => npc == null);
            }
        }
    }
}
