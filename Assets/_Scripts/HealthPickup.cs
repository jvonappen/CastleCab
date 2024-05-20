using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthIncrease = 1;

    [SerializeField] private float spinValue = 5;

    private void Update()
    {
        transform.Rotate(0, spinValue, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            //AudioManager.Instance.PlaySFX("Money");
            Destroy(gameObject);
        }
    }
}
