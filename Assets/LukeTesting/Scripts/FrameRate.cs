using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
