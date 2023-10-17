using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] private Canvas _achievementCanvas;
    [SerializeField] private TextMeshProUGUI _achievementText;

    [Header("Achievement")]
    public bool unlockPegasus = false;
    public bool unlockSmoothCriminal = false;
    public bool unlockBaconEggs = false;
    public bool unlockCollidesdale = false;
    public bool unlockBaaBoom = false;
    public bool unlockPloughHorse = false;
    public bool unlockAwakened = false;
    public bool unlockSundayService = false;
    public bool unlockShowPony = false;
    public bool unlockPlatniumDriver = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _achievementCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayAchievment()
    {
        _achievementCanvas.enabled = true;
    }

    public void HideAchievment()
    {
        _achievementCanvas.enabled = false;     
    }

    private void Pegasus()  
    { /* Airtime of (5) seconds */
        
        _achievementText.text = "Pegasus";
    }
    private void SmoothCriminal() 
    { /* Remove max wanted level */
        unlockSmoothCriminal = true;
        _achievementText.text = "Smooth Criminal";
    }
    private void BaconEggs() 
    { /* Kill a chicken and pig within (5) seconds */
        unlockBaconEggs = true;
        _achievementText.text = "Bacon and Eggs";
    }
    private void Collidesdale() 
    { /* Break 100 objects */
        unlockCollidesdale = true;
        _achievementText.text = "Collidesdale";
    }
    private void BaaBoom() 
    { /* Find the barrel sheep */
        unlockBaaBoom = true;
        _achievementText.text = "Baa Boom!";
    }
    private void PloughHorse() 
    { /* Destroy (X) amount of fences */
        unlockPloughHorse = true;
        _achievementText.text = "Plough Horse";
    }
    private void Awakened() 
    { /* Unlock gold skin */
        unlockAwakened = true;
        _achievementText.text = "Awakened";
    }
    private void SundayService() 
    { /* Get into the church */
        unlockSundayService = true;
        _achievementText.text = "Sunday Service";
        DisplayAchievment();
    }
    private void ShowPony() 
    { /* Change appearance */
        unlockShowPony = true;
        _achievementText.text = "Show Pony";
    }
    private void PlatniumDriver() 
    { /* Complete all quests */
        unlockPlatniumDriver = true;
        _achievementText.text = "Platnium Driver";
    }

    private void CheckAchievement()
    {
        if(unlockPegasus == true) { Pegasus(); }
    }

}
