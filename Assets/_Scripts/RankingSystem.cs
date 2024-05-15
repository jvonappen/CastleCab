using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public static RankingSystem Instance;
    public List<WagonData> players;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }          
    }

    public void UpdateRanking()
    {
        Debug.Log(players.Count);

        //CalculateRanks();
        UpdateRankUI();
       
    }

    private void CalculateRanks()
    {
        players = players.OrderBy(x => x.score.scoreValue).ToList();
    }

    private void UpdateRankUI()
    {
        if (players.Count == 1) { players[0].currentRankUI = players[0].rankingNumberUI[0]; players[0].currentRankUI.SetEnabled(true); }
        if (players.Count == 2) { players[1].currentRankUI = players[1].rankingNumberUI[1]; players[1].currentRankUI.SetEnabled(true); } 
        if (players.Count == 3) { players[2].currentRankUI = players[2].rankingNumberUI[2]; players[2].currentRankUI.SetEnabled(true); }   
        if (players.Count == 4) { players[3].currentRankUI = players[3].rankingNumberUI[3]; players[3].currentRankUI.SetEnabled(true); }
        

    }
   



}
