using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject npcPrefabs;
    [SerializeField] GameObject guardPrefabs;
    [SerializeField] Transform respawnPoint;
    [SerializeField] float respawnTime;
    [SerializeField] int maxNPCs = 40;
    [SerializeField] int maxGuards = 15;

    void Awake()
    {
        SpawnNPC();
        SpawnGuards();
    }

    private void SpawnGuards()
    {
        int currentGuards = GameObject.FindGameObjectsWithTag("Guards").Length;

        if (currentGuards < maxGuards)
        {
            Instantiate(guardPrefabs, respawnPoint.position, respawnPoint.rotation);
        }

        Invoke("SpawnGuards", respawnTime);
    }

    void SpawnNPC()
    {
        int currentNPCs = GameObject.FindGameObjectsWithTag("NPC").Length;

        if (currentNPCs < maxNPCs)
        {
            Instantiate(npcPrefabs, respawnPoint.position, respawnPoint.rotation);
        }

        Invoke("SpawnNPC", respawnTime);


    }
}
