using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestTargetType
{
    Kill,
    Gather
}
public class QuestTarget : MonoBehaviour
{
    public QuestTargetType questTargetType;

    public string targetName;

    [Header("Debug")]
    [SerializeField] private int m_indexNumber;
    [SerializeField] private GameObject target;

    private void Awake()
    {
        target = this.gameObject;

        if(questTargetType == QuestTargetType.Kill)
        {
          

            //indexNumber = Array.FindIndex(QuestManager.Instance.enemies, 

        }
    }


    //private void GetIndexNumber(string name, ArrayList array)
    //{
    //    return array.IndexOf(name) > -1;

    //}
}
