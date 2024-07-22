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
    public int amountRequired;
    public int current;

    public bool IsReached()   { return (current >= amountRequired); }

    public void ObjectiveKilled() //Add a tag/index argument for specific items or enemies
    {
        if(questType == QuestType.Kill)
        {
            current++;
        }
    }

    public void Gathered()
    {
        if (questType == QuestType.Gather)
        {
            current++;
        }
    }
}


