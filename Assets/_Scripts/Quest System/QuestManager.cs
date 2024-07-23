using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager Instance;
    void CreateSingleton()
    {
        if (Instance) Destroy(gameObject);
        else
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [Header("Quest Debug")]
    public Quest quest;



    [Header("Quest Enemy Index List")]
    public QuestTarget[] enemies;
    [Header("Quest Item Index List")]
    public string[] gatherItems;

    private void Awake()
    {
        CreateSingleton();
    }
}



//List of Enemies
//List of Gather Items
//List or Reward Items - Cosmetic/Dye/PowerUP/???