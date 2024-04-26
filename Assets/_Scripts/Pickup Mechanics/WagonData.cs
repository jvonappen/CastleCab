using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonData : MonoBehaviour
{
    [SerializeField] private GameObject wagonSlotPoint;

    public bool isOccupied;
    public  GameObject wagonSlot;
    public  GameObject destinationTarget;
    public Score score;

    private void Awake()
    {
        isOccupied = false;
        wagonSlot = wagonSlotPoint;
    }

}
