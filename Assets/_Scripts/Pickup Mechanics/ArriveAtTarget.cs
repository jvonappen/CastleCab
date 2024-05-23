using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtTarget : MonoBehaviour
{
    
    [SerializeField] private GameObject exitLocation;

    [SerializeField] private bool m_canVanish = true;
    [SerializeField] private GameObject m_vanishParticles;
    [SerializeField] private float m_vanishTimer = 10;

    private WagonService _wagonContents;
    private WagonData _wagonData;
    private GameObject _cartTargetPoint;

    private void Awake()
    {
        m_vanishParticles.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;

        _wagonData = other.GetComponent<WagonData>();
        _cartTargetPoint = _wagonData.wagonSlot;
        if(!_wagonData.isOccupied) return;
        _wagonContents = _cartTargetPoint.GetComponentInChildren<WagonService>();
        

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
            
            _wagonContents.thisPlayerMarker.SetActive(false);
            _wagonContents.thisPlayerBeam.SetActive(false);
            _wagonData.score.scoreValue = _wagonData.score.scoreValue + _wagonContents.scoreGiven;


            _wagonContents.transform.parent = null;
            _wagonContents.transform.position = exitLocation.transform.position;
            _wagonContents.isAtTarget = true;
            _wagonData.isOccupied = false;
            _wagonData.destinationTarget = null;
            _wagonContents.destination = null;

        }

        if(_wagonContents.isAtTarget)
        { 
            _wagonContents.ChangeAnimation("Dance");
            if(m_canVanish)
            {
                TimerManager.RunAfterTime(() =>
                {
                    if (m_vanishParticles != null){m_vanishParticles.SetActive(true);}
                    _wagonContents.gameObject.SetActive(false);
                }, m_vanishTimer);
            }

        }
        

    }
}
