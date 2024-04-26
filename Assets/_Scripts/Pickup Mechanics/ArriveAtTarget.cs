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

    //[SerializeField] private Score thisScore;
    [SerializeField] private WagonData _wagonData;

    // [Header("Timer")]
    //[SerializeField] private GameObject timerObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;

        ////if (wagonContents != null) { Debug.Log("Contents check"); }
        ////if (wagonContents.destination == this.gameObject) { Debug.Log("Destination check"); }
        ////if (other.tag == "Wagon") { Debug.Log("Tag check"); }

        _wagonData = other.GetComponent<WagonData>();

        cartTargetPoint = _wagonData.wagonSlot;

        wagonContents = cartTargetPoint.GetComponentInChildren<WagonService>();

        //wagonContents = _wagonData.wagonSlot.GetComponentInChildren<WagonService>();
   
      
        
   



        if (wagonContents != null && wagonContents.destination == this.gameObject && other.tag == "Wagon")
        {
            //AudioManager.Instance.StopSFX();
            wagonContents.transform.parent = null;
            wagonContents.transform.position = exitLocation.transform.position;

            _wagonData.isOccupied= false;
            _wagonData.destinationTarget = null;


            //DollarDisplay.dollarValue = DollarDisplay.dollarValue + wagonContents.dollarsGiven;

            _wagonData.score.scoreValue = _wagonData.score.scoreValue + wagonContents.scoreGiven;

            

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
