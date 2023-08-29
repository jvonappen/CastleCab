using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtObjective : MonoBehaviour
{
    [SerializeField] private GameObject cartTargetPoint;
    [SerializeField] private GameObject exitLocation;

    private TaxiService taxiPassenger;

    [SerializeField] private Canvas minimapMarker; //temp

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
            
            CartDetails.isOccupied= false;
            CartDetails.cartDestinationTarget = null;
            CompassBar.objectiveObjectTransform = null;

            DollarDisplay.dollarValue = DollarDisplay.dollarValue + taxiPassenger.dollarsGiven;
            AudioManager.Instance.PlaySFX("Money");

            minimapMarker.enabled = false; //temp

        }
    }
}
