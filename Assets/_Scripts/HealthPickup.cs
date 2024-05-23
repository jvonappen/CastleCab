using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthIncrease = 1;

    [SerializeField] private float spinValue = 5;
    [SerializeField] private float respawnTime = 3;



   private WagonData _wagonData;

    private void Update()
    {
        transform.Rotate(0, spinValue, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon" || other.tag != "Player") return;    
            
        _wagonData = other.GetComponent<WagonData>();

        _wagonData.playerHealth.HealthPickupIncrease(healthIncrease);

        //AudioManager.Instance.PlaySFX("Money");
        //Destroy(gameObject);
       
        gameObject.SetActive(false);
        _wagonData.PlayPickUpParticle();

        TimerManager.RunAfterTime(() =>
        {
            gameObject.SetActive(true);          
        }, respawnTime);

        
    }
}
