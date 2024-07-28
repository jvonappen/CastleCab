using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Target", menuName = "Quest Target Details")]

public class QuestTargetDetails : ScriptableObject
{
    public QuestTargetType questTargetType;
    public string targetName;

    public enum QuestTargetType
    {
        Kill,
        Gather
    }
}
