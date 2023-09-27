using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintYeWagon : MonoBehaviour
{
    [SerializeField] private GameObject wagon;
    [SerializeField] private int cost;
    [SerializeField] private Canvas paintYeWagonCanvas;

    [SerializeField] private Paint _paintData;
    private Material tempMat;

    [SerializeField] ParticleSystem _paintYeWagonParticle;
    private static Transform _particlePos;
    private void Start()
    {
        paintYeWagonCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _particlePos = other.transform;
            PaintMeWagon();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        paintYeWagonCanvas.enabled = false;
    }

    void PaintMeWagon()
    {
       

        if (Dishonour.dishonourLevel >= Dishonour._oneStar && DollarDisplay.dollarValue >= cost)
        {
            wagon.GetComponent<Renderer>().material = _paintData.material[Random.Range(0, 4)];
            PlayParticle();
            Dishonour.dishonourLevel = 0;
            DollarDisplay.dollarValue = DollarDisplay.dollarValue - cost;
        }
        if (Dishonour.dishonourLevel >= Dishonour._oneStar && DollarDisplay.dollarValue < cost)
        {
            paintYeWagonCanvas.enabled = true;
        }
    }

    public void PlayParticle()
    {
        _paintYeWagonParticle.transform.position = _particlePos.position;
        _paintYeWagonParticle.Play();
    }

}
