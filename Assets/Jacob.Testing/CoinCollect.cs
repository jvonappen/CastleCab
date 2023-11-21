using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void Update()
    {
        transform.Rotate(0, 5f, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DollarDisplay.dollarValue = DollarDisplay.dollarValue + coinValue;
            AudioManager.Instance.PlaySFX("Collectable");
            Destroy(gameObject);
        }       
    }
}
