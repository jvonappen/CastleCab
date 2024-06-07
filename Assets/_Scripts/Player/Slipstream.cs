using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TrailData
{
    public TrailData(Vector3 _pos, Vector3 _dir)
    {
        position = _pos;
        direction = _dir;
    }
    public Vector3 position, direction;
}

public class Slipstream : MonoBehaviour
{
    [SerializeField] int m_listCap = 20;
    [SerializeField] float m_spawnRate = 0.3f;

    [SerializeField] Transform m_horse;

    [SerializeField] List<TrailData> m_trailList = new();
    public List<TrailData> trailList { get { return m_trailList; } }

    private void Awake() { SpawnTrailSegment(); }

    void SpawnTrailSegment()
    {
        if (m_trailList.Count >= m_listCap) m_trailList.RemoveAt(0);
        
        TrailData td = new(m_horse.position, m_horse.forward);
        m_trailList.Add(td);
        
        //DebugSpawnTrailSegmentAsObject(td);
        
        TimerManager.RunAfterTime(SpawnTrailSegment, m_spawnRate);
    }

    void DebugSpawnTrailSegmentAsObject(TrailData _trailData)
    {
        GameObject obj = new("DebugTrailSegment");

        obj.transform.position = _trailData.position;
        obj.transform.forward = _trailData.direction;

        Destroy(obj, m_spawnRate * m_listCap);
    }
}
