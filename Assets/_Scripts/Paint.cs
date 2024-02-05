using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Paint", menuName = "Paint Data")]
public class Paint : ScriptableObject
{
    public string[] PaintJobName;
    public Material[] material;
}
