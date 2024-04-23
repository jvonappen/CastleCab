using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtTarget : MonoBehaviour
{
    [SerializeField] private GameObject cartTargetPoint;
    [SerializeField] private GameObject exitLocation;
    //[SerializeField] public GameObject targetParticles;

    private WagonService wagonContents;

    // [Header("Timer")]
    //[SerializeField] private GameObject timerObject;

    private void OnTriggerEnter(Collider other)
    {
        //wagonContents = cartTargetPoint.GetComponentInChildren<WagonService>(); **********************

        ////if (wagonContents != null) { Debug.Log("Contents check"); }
        ////if (wagonContents.destination == this.gameObject) { Debug.Log("Destination check"); }
        ////if (other.tag == "Wagon") { Debug.Log("Tag check"); }

        cartTargetPoint = WagonData.wagonSlot;

        wagonContents = WagonData.wagonSlot.GetComponentInChildren<WagonService>();

        if (wagonContents != null && wagonContents.destination == this.gameObject && other.tag == "Wagon")
        {
            //AudioManager.Instance.StopSFX();
            wagonContents.transform.parent = null;
            wagonContents.transform.position = exitLocation.transform.position;
            
            WagonData.isOccupied= false;
            WagonData.destinationTarget = null;
            

            //DollarDisplay.dollarValue = DollarDisplay.dollarValue + wagonContents.dollarsGiven;
           // AudioManager.Instance.PlaySFX("Money");

            //AchievementManager.platniumTracker = AchievementManager.platniumTracker + 1;
            //AchievementManager.Instance.PlatniumDriver();

            wagonContents.destination = null;

           // timerObject.SetActive(false);

            wagonContents.isAtTarget = true;

            //wagonContents.SetActive(false);

           // wagonContents.ResetTaxiPickUp();
            

        }
    }
}
