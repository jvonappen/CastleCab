using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public float timerValue = 30;
    [SerializeField] private GameObject _failedUI;

    [Header("Passenger")]
    [SerializeField] private GameObject _seat;
    //[SerializeField] private GameObject exitLocation;
    [SerializeField] ParticleSystem _explosiveImpact;
    private static Transform _particlePos;
    private TaxiService _taxi;
    //private GameObject _passenger;

    [Header("Debug")]
    [SerializeField] private bool hasFailed = false;
    [SerializeField] private float ticker = 0;
    private float _hideAfter = 3;
    public bool inService = false;

    private void Awake()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();

        inService = false;
        _failedUI.SetActive(false);
        hasFailed = false;
    }
    void Update()
    {
        if(timerValue > 0 && inService == true)
        {
            _taxi = _seat.GetComponentInChildren<TaxiService>();
            timerText.enabled = true;
            timerText.color = Color.white;
            timerValue = timerValue - 1 * Time.deltaTime;
            timerText.text = timerValue.ToString("00");
        }
        if(timerValue <= 0 && inService == true)
        {
            timerText.enabled = false;

            inService = false;

            _failedUI.SetActive(true);
            hasFailed = true;
            _particlePos = _seat.transform;
            PlayParticle(_explosiveImpact);
            FailedDelivery();

        }
        if(hasFailed == true)
        {
            
            ticker = ticker + 1 * Time.deltaTime;
            
        }
        if(ticker >= _hideAfter) { hasFailed = false; _failedUI.SetActive(false); ticker = 0; }
        

    }

    private void FailedDelivery()
    {
        
        if (_taxi != null)
        {
            AudioManager.Instance.PlaySFX("Out");        

            PlayerData.isOccupied = false;
            PlayerData.cartDestinationTarget = null;
            CompassBar.objectiveObjectTransform = null;

            _taxi.destination = null;    
            
            _taxi.targetParticles.SetActive(false);
            
            Destroy(_taxi.gameObject);
        }
    }
    private void PlayParticle(ParticleSystem particle)
    {
        particle.transform.position = _particlePos.position;
        particle.Play();
    }
}

