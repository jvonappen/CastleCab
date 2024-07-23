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

    private void Awake()
    {
        CreateSingleton();
    }
}
