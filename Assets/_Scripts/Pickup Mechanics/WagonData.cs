using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class WagonData : MonoBehaviour
{
    [SerializeField] private GameObject wagonSlotPoint;

    public bool isOccupied;
    [HideInInspector] public  GameObject wagonSlot;
    [HideInInspector] public GameObject destinationTarget;
    public Score score;
    public int thisPlayerNumber;
    public static int playerNumber = 0;
    //public GameObject[] rankingNumberUI;
    //public GameObject currentRankUI;

    //[SerializeField] private GameObject rankingPOS_L;
    //[SerializeField] private GameObject rankingPOS_R;


    private void Awake()
    {
        isOccupied = false;
        wagonSlot = wagonSlotPoint;
        playerNumber = playerNumber + 1;
        thisPlayerNumber = playerNumber;
        score.scoreValue = playerNumber;
       
       // RankingSystem.Instance.players.Add(this);
        MapScreenLocation.Instance.MapPosUpdate();
      //  RankingSystem.Instance.UpdateRanking();

        //SetRankingPOS();

        
    }

    //private void SetRankingPOS()
    //{
    //    if (thisPlayerNumber == 1) { currentRankUI.transform.position = rankingPOS_L.transform.position; }
    //    if (thisPlayerNumber == 2) { currentRankUI.transform.position = rankingPOS_R.transform.position; }
    //    if (thisPlayerNumber == 3) { currentRankUI.transform.position = rankingPOS_L.transform.position; }
    //    if (thisPlayerNumber == 4) { currentRankUI.transform.position = rankingPOS_R.transform.position; }
    //}

}
