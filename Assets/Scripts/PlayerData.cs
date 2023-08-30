using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static bool isOccupied;
    public static GameObject player;
    public GameObject customerSeat;
    public static GameObject cartDestinationTarget;

    [Header("Debug")]
    public GameObject debugCartDestination;

    private void Awake()
    {
        isOccupied = false;
        player = this.gameObject;
    }

    private void Update()
    {
        debugCartDestination = cartDestinationTarget;
    }

}
