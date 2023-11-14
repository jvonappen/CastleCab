using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtObjective : MonoBehaviour
{
    [SerializeField] private GameObject cartTargetPoint;
    [SerializeField] private GameObject exitLocation;
    [SerializeField] public GameObject targetParticles;

    private TaxiService taxiPassenger;

    [Header("Timer")]
    [SerializeField] private GameObject timerObject;

    private void OnTriggerEnter(Collider other)
    {
        taxiPassenger = cartTargetPoint.GetComponentInChildren<TaxiService>();
        if(taxiPassenger != null && taxiPassenger.destination == this.gameObject && other.tag == "Player")
        {
            AudioManager.Instance.StopSFX();
            taxiPassenger.transform.parent = null;
            taxiPassenger.transform.position = exitLocation.transform.position;
            
            PlayerData.isOccupied= false;
            PlayerData.cartDestinationTarget = null;
            CompassBar.objectiveObjectTransform = null;

            DollarDisplay.dollarValue = DollarDisplay.dollarValue + taxiPassenger.dollarsGiven;
            AudioManager.Instance.PlaySFX("Money");

            taxiPassenger.destination = null;

            timerObject.SetActive(false);

            taxiPassenger.isAtTarget = true;

            targetParticles.SetActive(false);

        }
    }
}
