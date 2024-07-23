using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Kill,
    Gather
}

//If Kill QuestType select int from Kill Target List
//If Gather QuestType select int from Gather Target List

[System.Serializable]
public class QuestObjectives
{
    public QuestType questType;
    public int targetsIndexNumber; //Add Note: Uses the number from the list of targets for each Quest Type
    public int amountRequired;
    public int current;

    private string[] indexList;

    public bool IsReached()   { return (current >= amountRequired); }

    public void ObjectiveKilled(int indexNum) //Add a tag/index argument for specific items or enemies
    {
        if(questType == QuestType.Kill)
        {
            indexList = QuestManager.Instance.enemies;

            

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


