using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtTarget : MonoBehaviour
{
    
    [SerializeField] private GameObject exitLocation;
    //[SerializeField] public GameObject targetParticles;

    

    private WagonService _wagonContents;
    private WagonData _wagonData;
    private GameObject _cartTargetPoint;

    //[SerializeField] private Score thisScore;

    // [Header("Timer")]
    //[SerializeField] private GameObject timerObject;

    private void Awake()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;

        _wagonData = other.GetComponent<WagonData>();
        _cartTargetPoint = _wagonData.wagonSlot;
        if(!_wagonData.isOccupied) return;
        _wagonContents = _cartTargetPoint.GetComponentInChildren<WagonService>();
        _wagonContents.thisPlayerMarker.SetActive(false);

        if (_wagonContents.captureFlagToggle == true && _wagonContents != null && _wagonContents.destination == this.gameObject)
        {
            _wagonData.score.scoreValue = _wagonData.score.scoreValue + _wagonContents.scoreGiven;
            _wagonContents.transform.parent = null;
            _wagonContents.transform.position = exitLocation.transform.position;
            _wagonContents.isAtTarget = true;
            _wagonData.isOccupied = false;
            _wagonData.destinationTarget = null;
            _wagonContents.destination = null;          
            _wagonContents.gameObject.SetActive(false);
        }

        if (_wagonContents.zonedDeliveriesToggle == true && _wagonContents != null && _wagonContents.destination == this.gameObject)
        {
            _wagonData.score.scoreValue = _wagonData.score.scoreValue + _wagonContents.scoreGiven;
            _wagonContents.transform.parent = null;
            _wagonContents.transform.position = exitLocation.transform.position;
            _wagonContents.isAtTarget = true;
            _wagonData.isOccupied = false;
            _wagonData.destinationTarget = null;
            _wagonContents.destination = null;
            _wagonContents.gameObject.SetActive(false);
        }

        if (!_wagonContents.captureFlagToggle && _wagonContents != null && _wagonContents.destination == this.gameObject)
        {
            //AudioManager.Instance.StopSFX();
            _wagonContents.transform.parent = null;
            _wagonContents.transform.position = exitLocation.transform.position;

            _wagonData.isOccupied = false;
            _wagonData.destinationTarget = null;


            //DollarDisplay.dollarValue = DollarDisplay.dollarValue + wagonContents.dollarsGiven;

            _wagonData.score.scoreValue = _wagonData.score.scoreValue + _wagonContents.scoreGiven;



            // AudioManager.Instance.PlaySFX("Money");

            //AchievementManager.platniumTracker = AchievementManager.platniumTracker + 1;
            //AchievementManager.Instance.PlatniumDriver();

            _wagonContents.destination = null;

            // timerObject.SetActive(false);

            _wagonContents.isAtTarget = true;

            //wagonContents.SetActive(false);

            // wagonContents.ResetTaxiPickUp();


        }

        

    }
}
