using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType
{
    Statistic
}

[CreateAssetMenu(menuName = "Achievement", fileName = "Achievement")]
public class SO_Achievement : ScriptableObject
{
    public int ID = 0;

    [Header("Completion Requirements")]
    public AchievementType AchievementType;


    #region Statistic
    [ConditionalEnumHide("AchievementType", 0)] public Statistic Statistic;
    [ConditionalEnumHide("AchievementType", 0)] public float AmountForCompletion;
    #endregion



    // int currencyReward

    [Header("Display")]
    public string DisplayName = "{Achievement Name}";
    public string Description = "Go to ___ and ___";

    public Texture2D Icon;
}
