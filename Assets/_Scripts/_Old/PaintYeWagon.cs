using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintYeWagon : MonoBehaviour
{
    //[SerializeField] private GameObject wagon;
    [SerializeField] private int _removeDishonourCost = 50;
    [SerializeField] private int _paintJobCost = 10;
    [SerializeField] private Canvas paintYeWagonCanvas;

    //[SerializeField] private Paint _paintData;
    //private Material tempMat;

    [SerializeField] ParticleSystem _paintYeWagonParticle;
    private static Transform _particlePos;

    [SerializeField] private ParticleSystem _bigSpray;

    //private int currentColourIndex;
    //private int minValue;
    //private int maxValue;


    private void Start()
    {
        paintYeWagonCanvas.enabled = false;
        //minValue = 0;
        //maxValue = _paintData.material.Length;
        
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

        if (DollarDisplay.dollarValue >= _paintJobCost)
        {
            //wagon.GetComponent<Renderer>().material = _paintData.material[Random.Range(0, 4)];

            //wagon.GetComponent<Renderer>().material = _paintData.material[currentColourIndex];

            PlayParticle();
            AudioManager.Instance.PlaySFX("PaintYeWagon");
            DollarDisplay.dollarValue = DollarDisplay.dollarValue - _paintJobCost;
            _bigSpray.Play();
        }
        if (DishonourOld.dishonourLevel >= DishonourOld._fiveStar && DollarDisplay.dollarValue >= _removeDishonourCost)
        {
            PlayParticle();
            AudioManager.Instance.PlaySFX("PaintYeWagon");
            DishonourOld.dishonourLevel = 0;
            DollarDisplay.dollarValue = DollarDisplay.dollarValue - _removeDishonourCost;
            AchievementManagerOld.Instance.SmoothCriminal();
        }
        if (DishonourOld.dishonourLevel >= DishonourOld._oneStar && DollarDisplay.dollarValue >= _removeDishonourCost)
        {
            //wagon.GetComponent<Renderer>().material = _paintData.material[Random.Range(minValue, maxValue)];
            PlayParticle();
            AudioManager.Instance.PlaySFX("PaintYeWagon");
            DishonourOld.dishonourLevel = 0;
            DollarDisplay.dollarValue = DollarDisplay.dollarValue - _removeDishonourCost;
        }
        if (DishonourOld.dishonourLevel >= DishonourOld._oneStar && DollarDisplay.dollarValue < _removeDishonourCost)
        {
            paintYeWagonCanvas.enabled = true;
        }

    }

    public void PlayParticle()
    {
        _paintYeWagonParticle.transform.position = _particlePos.position;
        _paintYeWagonParticle.Play();
    }

    private void GetRandomColour()
    {
        //int randomColour = Random.Range(0, 4);

        //if(currentColourIndex == randomColour)
        //{

        //}

        //if(currentColourIndex != randomColour)
        //{
        //    currentColourIndex = randomColour;
        //}
    }

}
