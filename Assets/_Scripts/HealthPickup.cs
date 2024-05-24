using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthIncrease = 1;

    [SerializeField] private float spinValue = 5;
    [SerializeField] private float respawnTime = 3;



   private WagonData m_wagonData;
    private PlayerHealth m_playerHealth;

    private void Update()
    {
        transform.Rotate(0, spinValue, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wagon")
        {
            m_wagonData = other.GetComponent<WagonData>();
            m_wagonData.playerHealth.HealthPickupIncrease(healthIncrease);
            AudioManager.Instance.PlaySFX("HealthPickup");
            gameObject.SetActive(false);
            m_wagonData.PlayPickUpParticle();
            TimerManager.RunAfterTime(() =>{ gameObject.SetActive(true); }, respawnTime);
        }

        //if (other.tag == "Player")
        //{
        //    m_playerHealth = other.attachedRigidbody.GetComponentInParent<PlayerHealth>();
        //    m_playerHealth.HealthPickupIncrease(healthIncrease);
        //    //AudioManager.Instance.PlaySFX("Money");
        //    gameObject.SetActive(false);
        //    m_wagonData.PlayPickUpParticle();
        //    TimerManager.RunAfterTime(() => { gameObject.SetActive(true); }, respawnTime);
        //}

    }
}
