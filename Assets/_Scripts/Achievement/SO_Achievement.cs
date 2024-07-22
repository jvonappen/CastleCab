using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement", fileName = "Achievement")]
public class SO_Achievement : ScriptableObject
{
    public int ID = 0;

    public string DisplayName = "{Achievement Name}";
    public string Description = "Go to ___ and ___";

    public Texture2D Icon;

    // int currencyReward
}
