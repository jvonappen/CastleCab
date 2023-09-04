using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartDetails : MonoBehaviour
{
    public static bool isOccupied;

    public static GameObject cartDestinationTarget;

    [Header("Debug")]
    public GameObject debugCartDestination;
    void Start()
    {
        isOccupied = false;
    }

    private void Update()
    {
        debugCartDestination = cartDestinationTarget;
    }

}
