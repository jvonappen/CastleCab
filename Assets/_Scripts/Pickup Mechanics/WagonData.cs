using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WagonData : MonoBehaviour
{
    [SerializeField] private GameObject wagonSlotPoint;

    public bool isOccupied;
    public  GameObject wagonSlot;
    public  GameObject destinationTarget;
    public Score score;

    public int thisPlayerNumber;
    public static int playerNumber = 0;
    private void Awake()
    {
        isOccupied = false;
        wagonSlot = wagonSlotPoint;
        playerNumber = playerNumber + 1;
        thisPlayerNumber = playerNumber;
    }

}
