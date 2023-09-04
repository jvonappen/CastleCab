using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenNoise : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySFX("Chicken");
        }
    }
}
