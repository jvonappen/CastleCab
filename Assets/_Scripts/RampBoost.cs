using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBoost : MonoBehaviour
{
    [SerializeField] private float boostSpeed;

    private PlayerMovement playerMovement;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        playerMovement = other.GetComponent<PlayerMovement>();

      
        

    }
}
