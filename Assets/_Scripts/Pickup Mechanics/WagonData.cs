using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WagonData : MonoBehaviour
{
    [SerializeField] private GameObject wagonSlotPoint;

    [SerializeField] public PlayerHealth playerHealth;

   public bool isOccupied;
    [HideInInspector] public  GameObject wagonSlot;
    [HideInInspector] public GameObject destinationTarget;

    public Score score;

    [Header("Particles")]
    [SerializeField] private GameObject pickupParticle;

    [Header("Debug")]
    public int thisPlayerNumber;
    public static int playerNumber = 0;

    private void Awake()
    {
        pickupParticle.SetActive(false);
        isOccupied = false;
        wagonSlot = wagonSlotPoint;
        playerNumber = playerNumber + 1;
        thisPlayerNumber = playerNumber;
        score.scoreValue = playerNumber;
       
        MapScreenLocation.Instance.MapPosUpdate();  
    }

    public void PlayPickUpParticle()
    {
        pickupParticle.SetActive(true);

        TimerManager.RunAfterTime(() =>
        {
            pickupParticle.SetActive(false);
        }, 1.5f);
    }

}
