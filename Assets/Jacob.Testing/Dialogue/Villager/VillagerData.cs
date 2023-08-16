using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Villager", menuName = "Villager")]
public class VillagerData : ScriptableObject
{
    [SerializeField] private string villagerName;
    public Color villagerColor;
    public Color villagerNameColor;
    public DialogueData dialogue;
}
