using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public static RankingSystem Instance;

    [SerializeField] public WagonData P1, P2, P3, P4;

    [Header("Ranking Position UI")]
    [SerializeField] private GameObject ui_first, ui_second, ui_third, ui_fourth;

    
    //Transform Positions for 1v1 or 1vMore UI
    [SerializeField] private GameObject tposA_1v1, tposB_1v1;
    [SerializeField] private GameObject tposA_1vMore, tposB_1vMore, tposC_1vMore, tposD_1vMore;

    [Header("Debug")]
    [SerializeField] private int scoreP1, scoreP2, scoreP3, scoreP4;   

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

    public void CalculateRanking(WagonData player)
    {
        GetScore();

        player.rank = 0;

        if (player.score.scoreValue >= scoreP1) { player.rank = player.rank - 1; }
        if (player.score.scoreValue >= scoreP2) { player.rank = player.rank - 1; }
        if (player.score.scoreValue >= scoreP3) { player.rank = player.rank - 1; }
        if (player.score.scoreValue >= scoreP4) { player.rank = player.rank - 1; }

        if (player.score.scoreValue < scoreP1) { player.rank = player.rank + 1; }
        if (player.score.scoreValue < scoreP2) { player.rank = player.rank + 1; }
        if (player.score.scoreValue < scoreP3) { player.rank = player.rank + 1; }
        if (player.score.scoreValue < scoreP4) { player.rank = player.rank + 1; }

        SetRankingUI(player);
        RankingUIPlacement();
    }

    private void GetScore()
    {
        if (P1 != null) { scoreP1 = P1.score.scoreValue; }
        if (P2 != null) { scoreP2 = P2.score.scoreValue; }
        if (P3 != null) { scoreP3 = P3.score.scoreValue; }
        if (P4 != null) { scoreP4 = P4.score.scoreValue; }
    }

    private void SetRankingUI(WagonData player)
    {
        if (player.rank == 1) { player.rankingUI = ui_first;  }
        if (player.rank == 2) { player.rankingUI = ui_second; }
        if (player.rank == 3) { player.rankingUI = ui_third;  }
        if (player.rank == 4) { player.rankingUI = ui_fourth; }

        else Debug.Log("Ranking system error");
    }

    private void RankingUIPlacement()
    {
        if (P1 != null && P2 != null && P3 == null)
        {
            P1.rankingUI.transform.position = tposA_1v1.transform.position;
            P2.rankingUI.transform.position = tposB_1v1.transform.position;
        }
        if (P3 != null)
        {
            P1.rankingUI.transform.position = tposA_1vMore.transform.position;
            P2.rankingUI.transform.position = tposB_1vMore.transform.position;
            P3.rankingUI.transform.position = tposC_1vMore.transform.position;
        }
        if (P4 != null)
        {
            P4.rankingUI.transform.position = tposD_1vMore.transform.position;
        }
    }

}
