using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintYeWagon : MonoBehaviour
{
    [SerializeField] private GameObject wagon;
    [SerializeField] private int cost;
    [SerializeField] private Canvas paintYeWagonCanvas;

    private void Start()
    {
        paintYeWagonCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PaintMeWagon();
    }

    private void OnTriggerExit(Collider other)
    {
        paintYeWagonCanvas.enabled = false;
    }

    void PaintMeWagon()
    {
        if (Dishonour.dishonourLevel >= Dishonour._oneStar && DollarDisplay.dollarValue >= cost)
        {
            wagon.GetComponent<Renderer>().material.color = Color.green;
            Dishonour.dishonourLevel = 0;
            DollarDisplay.dollarValue = DollarDisplay.dollarValue - cost;
        }
        if (Dishonour.dishonourLevel >= Dishonour._oneStar && DollarDisplay.dollarValue < cost)
        {
            paintYeWagonCanvas.enabled = true;
        }
    }

 

}
