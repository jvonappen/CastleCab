using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.TimeZoneInfo;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] private Canvas _achievementCanvas;
    [SerializeField] private TextMeshProUGUI _achievementText;

    [Header("Achievements")]
    public bool unlockPegasus = false;
    public static float airTimeTick = 0;
    [SerializeField] private int airTimeNeeded = 5;

   public bool unlockSmoothCriminal = false;

    public bool unlockBaconEggs = false;

    public bool unlockCollidesdale = false;

    public bool unlockBaaBoom = false;

    public bool unlockPloughHorse = false;
    public static int fenceTracker = 0;
    [SerializeField] private int fencesNeeded = 50;

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
        //CheckAchievement();
    }

    public void DisplayAchievment()
    {
        _achievementCanvas.enabled = true;
        StartCoroutine(EndDisplayAchievment());
    }

    IEnumerator EndDisplayAchievment()
    {
        yield return new WaitForSeconds(5);
        _achievementCanvas.enabled = false;      
    }

    private void Pegasus()  
    { /* Airtime of (5) seconds */

        //airTimeTick= (airTimeTick + 1) * Time.deltaTime;

        //if(airTimeTick >= airTimeNeeded)
        //{
        //    unlockPegasus = true;
        //    DisplayAchievment();
        //    _achievementText.text = "Pegasus";
        //}
        
    }
    private void SmoothCriminal() 
    { /* Remove max wanted level */

        _achievementText.text = "Smooth Criminal";
    }
    private void BaconEggs() 
    { /* Kill a chicken and pig within (5) seconds */

        _achievementText.text = "Bacon and Eggs";
    }
    private void Collidesdale() 
    { /* Break 100 objects */
        
        _achievementText.text = "Collidesdale";
    }
    private void BaaBoom() 
    { /* Find the barrel sheep */
        
        _achievementText.text = "Baa Boom!";
    }
    public void PloughHorse() 
    { /* Destroy (X) amount of fences */
        
        if(fenceTracker >= fencesNeeded && unlockPloughHorse == false)
        {
            unlockPloughHorse = true;
            _achievementText.text = "Plough Horse";
            DisplayAchievment();
        }
        
    }
    private void Awakened() 
    { /* Unlock gold skin */
        
        _achievementText.text = "Awakened";
    }
    private void SundayService() 
    { /* Get into the church */
        
        _achievementText.text = "Sunday Service";
        DisplayAchievment();
    }
    private void ShowPony() 
    { /* Change appearance */
        
        _achievementText.text = "Show Pony";
    }
    private void PlatniumDriver() 
    { /* Complete all quests */
        
        _achievementText.text = "Platnium Driver";
    }

    //private void CheckAchievement()
    //{
    //    if(unlockPegasus == true) { Pegasus(); }
    //    if (unlockSmoothCriminal == true) { SmoothCriminal(); }
    //    if (unlockBaconEggs == true) { BaconEggs(); }
    //    if (unlockCollidesdale == true) { Collidesdale(); }
    //    if (unlockBaaBoom == true) { BaaBoom(); }
    //    if (unlockPloughHorse == true) { PloughHorse(); }
    //    if (unlockAwakened == true) { Awakened(); }
    //    if (unlockSundayService == true) { SundayService(); }
    //    if (unlockShowPony == true) { ShowPony(); }
    //    if (unlockPlatniumDriver == true) { PlatniumDriver(); }
    //}



}
