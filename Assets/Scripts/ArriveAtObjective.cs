using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtObjective : MonoBehaviour
{
    [SerializeField] private GameObject cartTargetPoint;

    [SerializeField] private GameObject exitLocation;

    [SerializeField] private VillagerDetails villager;

    [SerializeField] private Canvas minimapMarker; //temp

    private void Start()
    {
        minimapMarker.enabled = false; //temp
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello");
        villager = cartTargetPoint.GetComponentInChildren<VillagerDetails>();
        if(villager != null && villager.destination == this.gameObject)
        {
            AudioManager.Instance.PlaySFX("Out");
            villager.transform.parent = null;
            villager.transform.position = exitLocation.transform.position;
            
            CartDetails.isOccupied= false;
            CartDetails.cartDestinationTarget = null;
            CompassBar.objectiveObjectTransform = null;

            DollarDisplay.dollarValue = DollarDisplay.dollarValue + villager.dollarsGiven;
            AudioManager.Instance.PlaySFX("Money");

            minimapMarker.enabled = false; //temp

        }
    }
}
