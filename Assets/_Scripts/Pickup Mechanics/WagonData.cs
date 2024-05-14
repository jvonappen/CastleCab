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
    public GameObject rankingUI;
    private void Awake()
    {
        isOccupied = false;
        wagonSlot = wagonSlotPoint;
        playerNumber = playerNumber + 1;
        thisPlayerNumber = playerNumber;
        score.scoreValue = playerNumber;
        AddToRankingSystem();
        RankingSystem.Instance.players.Add(this);
        MapScreenLocation.Instance.MapPosUpdate();
        RankingSystem.Instance.UpdateRanking();
    }

    private void AddToRankingSystem()
    {
        if (playerNumber == 1) { RankingSystem.Instance.P1 = this; }
        if (playerNumber == 2) { RankingSystem.Instance.P2 = this; }
        if (playerNumber == 3) { RankingSystem.Instance.P3 = this; }
        if (playerNumber == 4) { RankingSystem.Instance.P4 = this; }
    }

}
