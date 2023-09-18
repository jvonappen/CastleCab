using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtObjective : MonoBehaviour
{
    [SerializeField] private GameObject cartTargetPoint;
    [SerializeField] private GameObject exitLocation;

    private TaxiService taxiPassenger;

    [SerializeField] public Canvas minimapMarker; //temp

    [Header("Timer")]
    [SerializeField] private GameObject timerObject;

    private void Start()
    {
        minimapMarker.enabled = false; //temp
    }
    private void OnTriggerEnter(Collider other)
    {
        taxiPassenger = cartTargetPoint.GetComponentInChildren<TaxiService>();
        if(taxiPassenger != null && taxiPassenger.destination == this.gameObject)
        {
            AudioManager.Instance.PlaySFX("Out");
            taxiPassenger.transform.parent = null;
            taxiPassenger.transform.position = exitLocation.transform.position;
            
            PlayerData.isOccupied= false;
            PlayerData.cartDestinationTarget = null;
            CompassBar.objectiveObjectTransform = null;

            DollarDisplay.dollarValue = DollarDisplay.dollarValue + taxiPassenger.dollarsGiven;
            AudioManager.Instance.PlaySFX("Money");

            taxiPassenger.destination = null;

            minimapMarker.enabled = false; //temp

            timerObject.SetActive(false);

        }
    }
}
