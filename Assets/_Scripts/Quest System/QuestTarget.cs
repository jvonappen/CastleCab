using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTarget : MonoBehaviour
{
    public QuestTargetDetails questTargetDetails;
    public string targetName;

    private void Awake()
    {
        targetName = questTargetDetails.name;
    }
    //do stuff here
}
