using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonData : MonoBehaviour
{
    [SerializeField] private GameObject wagonSlotPoint;

    public static bool isOccupied;
    public static GameObject wagonSlot;
    public static GameObject destinationTarget;

    

    [Header("Debug")]
    public GameObject debugCartDestination;

    private void Awake()
    {
        isOccupied = false;
        wagonSlot = wagonSlotPoint;
    }

    private void Update()
    {
        debugCartDestination = destinationTarget;
    }

}
