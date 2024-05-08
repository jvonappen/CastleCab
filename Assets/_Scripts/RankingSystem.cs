using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public static RankingSystem Instance;

    [SerializeField] public WagonData P1, P2, P3, P4;
    [SerializeField] private int scoreP1, scoreP2, scoreP3, scoreP4;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
       
    }

    public void UpdateRank(int score, WagonData player)
    {
        PlayerRank(score, player);

        if (P1 != null) DoTheRank(P1);
        if (P2 != null) DoTheRank(P2);
        if (P3 != null) DoTheRank(P3);
        if (P4 != null) DoTheRank(P4);

    }

    private void PlayerRank(int score, WagonData player)
    {

        int rank = 0;

        if (P1 != null && score > scoreP1) rank = +1;
        if (P2 != null && score > scoreP2) rank = +1;
        if (P3 != null && score > scoreP3) rank = +1;
        if (P4 != null && score > scoreP4) rank = +1;

        if (P1 != null && score < scoreP1) rank = -1;
        if (P2 != null && score < scoreP2) rank = -1;
        if (P3 != null && score < scoreP3) rank = -1;
        if (P4 != null && score < scoreP4) rank = -1;

        player.rank = rank;

    }

    private void DoTheRank(WagonData player)
    {
        ResetRankIcons(player);
        if (player.rank == 1) player.r1st.SetActive(true);
        if (player.rank == 2) player.r2nd.SetActive(true);
        if (player.rank == 3) player.r3rd.SetActive(true);
        if (player.rank == 4) player.r4th.SetActive(true);
    }

    private void ResetRankIcons(WagonData player)
    {
        player.r1st.SetActive(false);
        player.r2nd.SetActive(false);
        player.r3rd.SetActive(false);
        player.r4th.SetActive(false);

    }

}
