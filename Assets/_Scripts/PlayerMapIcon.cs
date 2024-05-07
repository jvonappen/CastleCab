using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapIcon : MonoBehaviour
{
    [SerializeField] private GameObject[] PlayerMapIcons;
    private int thisPlayerNumber;

    private void Awake()
    {
        thisPlayerNumber = WagonData.playerNumber;
        PlayerMapIcons[thisPlayerNumber].SetActive(true);
    }



}
