using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    
    public string  questTitle;
    public string  questDescription;
    public int goldReward;
    public int itemReward;

    public QuestObjectives questObjectives;

    public void QuestComplete()
    {
        isActive = false;
    }
}
