using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Kill,
    Gather
}


[System.Serializable]
public class QuestObjectives
{
    public QuestType questType;
    public QuestTarget questTarget;
    public int amountRequired;
    public int current;

    public bool IsReached()   { return (current >= amountRequired); }

    public void ObjectiveKilled(string targetName)
    {
        if(questType == QuestType.Kill && questTarget.targetName == targetName)
        {
            current++;
        }
    }

    public void ObjectiveGathered()
    {
        if (questType == QuestType.Gather)
        {
            current++;
        }
    }
}


