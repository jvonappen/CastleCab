using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArriveAtTarget : MonoBehaviour
{
    
    [SerializeField] private GameObject exitLocation;
    [SerializeField] private GameObject m_vanishParticles;

    private WagonService m_wagonContents;
    private WagonData m_wagonData;
    private GameObject m_cartTargetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;

        m_wagonData = other.GetComponent<WagonData>();
        m_cartTargetPoint = m_wagonData.wagonSlot;
        if(!m_wagonData.isOccupied) return;
        m_wagonContents = m_cartTargetPoint.GetComponentInChildren<WagonService>(); // Returns NPC reference in wagon
        

        //if (m_wagonContents.captureFlagToggle == true && m_wagonContents != null && m_wagonContents.destination == this.gameObject)
        //{
        //    m_wagonData.score.scoreValue = m_wagonData.score.scoreValue + m_wagonContents.scoreGiven;
        //    AudioManager.Instance.PlaySFX("Out");
        //    m_wagonContents.transform.parent = null;
        //    m_wagonContents.transform.position = exitLocation.transform.position;
        //    m_wagonContents.isAtTarget = true;
        //    m_wagonData.isOccupied = false;
        //    m_wagonData.destinationTarget = null;
        //    m_wagonContents.destination = null;          
        //    m_wagonContents.gameObject.SetActive(false);
        //}

        if (m_wagonContents.zonedDeliveriesToggle == true && m_wagonContents && m_wagonContents.destination == gameObject)
        {
            AudioManager.Instance.PlaySFX("Out");
            m_wagonContents.thisPlayerMarker.SetActive(false);
            m_wagonContents.thisPlayerBeam.SetActive(false);
            m_wagonData.score.scoreValue = m_wagonData.score.scoreValue + m_wagonContents.scoreGiven;

            m_wagonContents.transform.position = exitLocation.transform.position;
            m_wagonData.isOccupied = false;
            m_wagonData.destinationTarget = null;

            m_wagonContents.OnDropOff(m_vanishParticles);
        } 

    }
}
