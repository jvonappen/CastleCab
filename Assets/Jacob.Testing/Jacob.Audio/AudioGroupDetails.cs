using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group Audio", menuName = "Group Audio Data")]

public class AudioGroupDetails : ScriptableObject
{
    public string audioGroupName;
    public AudioClip[] audioClips;
}

