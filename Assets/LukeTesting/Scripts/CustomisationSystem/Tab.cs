using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public CustomisationOption _customisation;
    public List<string> tabOption;
    public List<Material> colorOption;
    public List<Texture2D> texture2D;
    public List<GameObject> modelOption;
    public List<Mesh> cartOption;
}

public enum CustomisationOption
{
    CartColour,
    HorseColour,
    WheelColour,
    HorseHat,
    CartModel
}