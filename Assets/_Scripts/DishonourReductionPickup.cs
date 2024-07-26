using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishonourReductionPickup : MonoBehaviour
{
    [SerializeField] private int dishonourDecrease = 100;

    [SerializeField] private float spinValue = 5;
    [SerializeField] private float respawnTime = 3;

    private WagonData m_wagonData;

    private void Update()
    {
        transform.Rotate(0, spinValue, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponentInParent<Dishonour>().DecreaseDishonour(dishonourDecrease);
        }
        
        if (other.tag == "Wagon")
        {
            m_wagonData = other.GetComponent<WagonData>();
            AudioManager.Instance.PlaySFX("DishonourDecrease");
            gameObject.SetActive(false);
            m_wagonData.PlayPickUpParticle();
            TimerManager.RunAfterTime(() => { gameObject.SetActive(true); }, respawnTime);
        }
    }
}
