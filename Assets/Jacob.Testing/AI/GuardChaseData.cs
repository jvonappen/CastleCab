using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guard Chase Data", menuName = "Guard Chase Data")]
public class GuardChaseData : ScriptableObject
{
    [Header("Dishonour Level One")]
    [Tooltip("Distance the AI will chase away the player.")]
    public float chasingRange1 = 5;
    [Tooltip("Speed modifier for a level one Dishonour Level.")]
    public float chaseSpeed1 = 5;
    [Tooltip("Distance the AI will search for the player.")]
    public float searchRange1 = 5;

    [Header("Dishonour Level Two")]
    [Tooltip("Distance the AI will chase away the player.")]
    public float chasingRange2 = 10;
    [Tooltip("Speed modifier for a level two Dishonour Level.")]
    public float chaseSpeed2 = 10;
    [Tooltip("Distance the AI will search for the player.")]
    public float searchRange2 = 10;

    [Header("Dishonour Level Three")]
    [Tooltip("Distance the AI will chase away the player.")]
    public float chasingRange3 = 20;
    [Tooltip("Speed modifier for a level three Dishonour Level.")]
    public float chaseSpeed3 = 15f;
    [Tooltip("Distance the AI will search for the player.")]
    public float searchRange3 = 15;
}
