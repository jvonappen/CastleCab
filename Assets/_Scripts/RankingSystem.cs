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

        Debug.Log(players.Count);
    }

    private void CalculateRanks()
    {
        players = players.OrderBy(x => x.score.scoreValue).ToList();
    }

    private void RankingUIPlacement()
    {
        if (players.Count == 1) { players[0].rankingUI = ui_first; }
        if (players.Count == 2)
        {
            ui_first.SetActive(true);
            ui_second.SetActive(true);
            ui_third.SetActive(false);
            ui_fourth.SetActive(false);

            players[0].rankingUI = ui_first;
            players[1].rankingUI = ui_second;

        }
        if (players.Count == 3)
        {
            ui_third.SetActive(true);
            players[0].rankingUI = ui_first;
            players[1].rankingUI = ui_second;
            players[2].rankingUI = ui_third;

        }
        if (players.Count == 4)
        {
            ui_fourth.SetActive(true);
            players[0].rankingUI = ui_first;
            players[1].rankingUI = ui_second;
            players[2].rankingUI = ui_third;
            players[3].rankingUI = ui_fourth;
        }

    }



}
