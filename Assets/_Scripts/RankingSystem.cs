using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public static RankingSystem Instance;

    [SerializeField] public WagonData P1, P2, P3, P4;
    public List<WagonData> players;

    [Header("Ranking Position UI")]
    [SerializeField] private GameObject ui_first;
    [SerializeField] private GameObject ui_second;
    [SerializeField] private GameObject ui_third;
    [SerializeField] private GameObject ui_fourth;


    [Header("Transform Positions 1v1")]
    [SerializeField] private GameObject posA_1v1;
    [SerializeField] private GameObject posB_1v1;

    [Header("Transform Positions 1vMore")]
    [SerializeField] private GameObject posA_1vMore;
    [SerializeField] private GameObject posB_1vMore;
    [SerializeField] private GameObject posC_1vMore;
    [SerializeField] private GameObject posD_1vMore;

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
        CalculateRanks();
        RankingUIPlacement();
    }

    private void CalculateRanks()
    {
        players = players.OrderBy(x => x.score.scoreValue).ToList();
    }

    private void RankingUIPlacement()
    {
        if (P1 != null && P2 != null && P3 == null)
        {
            ui_first.SetActive(true);
            ui_second.SetActive(true);
            ui_third.SetActive(false);
            ui_fourth.SetActive(false);

            players[0].rankingUI = ui_first;
            players[1].rankingUI = ui_second;

            P1.rankingUI.transform.position = posA_1v1.transform.position;
            P2.rankingUI.transform.position = posB_1v1.transform.position;
        }
        if (P3 != null)
        {
            ui_third.SetActive(true);
            players[0].rankingUI = ui_first;
            players[1].rankingUI = ui_second;
            players[2].rankingUI = ui_third;

            P1.rankingUI.transform.position = posA_1vMore.transform.position;
            P2.rankingUI.transform.position = posB_1vMore.transform.position;
            P3.rankingUI.transform.position = posC_1vMore.transform.position;
        }
        if (P4 != null)
        {
            ui_fourth.SetActive(true);
            players[0].rankingUI = ui_first;
            players[1].rankingUI = ui_second;
            players[2].rankingUI = ui_third;
            players[3].rankingUI = ui_fourth;

            P4.rankingUI.transform.position = posD_1vMore.transform.position;
        }

    }



}
