using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.TimeZoneInfo;
using UnityEngine.Animations.Rigging;
using System;
using UnityEngine.InputSystem;
using DG.Tweening;


public class AchievementManager : MonoBehaviour
{
    /* 
Achievements
     * Spa Day – Find the Pigs in mud baths 
     * Elderly Citizen’s Home – Deliver grandma 
    
 Collectables:  still missing
     * Letters [Castle Cab] **Castle Cab
     * Chicken Men ** The Colonel
     * Scarecrows ** Hay-man
     * Jesters  ** Funny Guy
     * Goblin Party

     */

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _achievementGameObject;

    public static AchievementManager Instance;

    [SerializeField] private Canvas _achievementCanvas;
    [SerializeField] private TextMeshProUGUI _achievementText;

    //Once off Achievements:

    [Header("SmoothCriminal")]
    private bool unlockSmoothCriminal = false;
    [Space]
    [Header("BaaBoom")]
    [SerializeField] private AchievementBoxDetail AT_BaBoom;
    public static bool unlockBaaBoom = false;
    [Space]
    [Header("ShowPony")]
    private bool unlockShowPony = false;
    [Space]
    [Header("BBC")]
    [SerializeField] private AchievementBoxDetail AT_BBC;
    private bool unlockBBC = false;
    [Space]
    [Space]
    [Header("Pegasus")]
    [SerializeField] private int airTricksNeeded = 5;
    [SerializeField] private AchievementBoxDetail AT_Pegasus;
    public bool unlockPegasus = false;          //Public for debug testing
    public static int airTrickTracker = 0;
    [Space]
    [Header("Collidesdale")]
    [SerializeField] private int stallsNeeded = 20;
    [SerializeField] private AchievementBoxDetail AT_Collidesdale;
    private bool unlockCollidesdale = false;
    public static int stallTracker = 0;
    [Space]
    [Header("PloughHorse")]
    [SerializeField] private int fencesNeeded = 50;
    [SerializeField] private AchievementBoxDetail AT_PloughHorse;
    private bool unlockPloughHorse = false;
    public static int fenceTracker = 0;
    [Space]
    [Header("PlatniumDriver")]
    [SerializeField] private int taxiNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_PlatniumDriver;
    private bool unlockPlatniumDriver = false;
    public static int platniumTracker = 0;
    [Space]
    [Header("Makin'Bacon")]
    [SerializeField] private int pigsNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_MakinBacon;
    private bool unlockMakinBacon = false;
    public static int pigTracker = 0;
    [Space]
    [Header("CluckMe")]
    [SerializeField] private int chickensNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_CluckMe;
    private bool unlockCluckMe = false;
    public static int cluckTracker = 0;
    [Space]
    [Header("GlueFactory")]
    [SerializeField] private int horsesNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_GlueFactory;
    private bool unlockGlueFactory = false;
    public static int glueTracker = 0;
    [Space]
    [Header("PublicMenace")]
    [SerializeField] private int npcsNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_PublicMenace;
    private bool unlockMenace = false;
    public static int menaceTracker = 0;
    [Space]
    [Header("GraveDigger")]
    [SerializeField] private int gravesNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_GraveDigger;
    private bool unlockGraveDigger = false;
    public static int graveTracker = 0;
    [Space]
    [Header("SpellingBee")]
    [SerializeField] private int lettersNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_SpellingBee;
    private bool unlockSpellingBee = false;
    public static int spellingTracker = 0;
    [Space]
    [Header("TheColonel")]
    [SerializeField] private int chickenmenNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_Colonel;
    private bool unlockColonel = false;
    public static int chickenmanTracker = 0;
    [Space]
    [Header("HayMan")]
    [SerializeField] private int scarecrowsNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_HayMan;
    private bool unlockHayMan = false;
    public static int scarecrowTracker = 0;
    [Space]
    [Header("FunnyGuy")]
    [SerializeField] private int jestersNeeded = 10;
    [SerializeField] private AchievementBoxDetail AT_FunnyGuy;
    private bool unlockFunnyGuy = false;
    public static int jesterTracker = 0;

    private UITween _uiTween;
    private bool _showUI;


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
        TrackerStartUp();
        
    }

    private void TrackerStartUp()
    {
        AT_Pegasus.SetTracker(airTricksNeeded);
        AT_Collidesdale.SetTracker(stallsNeeded);
        AT_PloughHorse.SetTracker(fencesNeeded);
        AT_PlatniumDriver.SetTracker(taxiNeeded);
        AT_MakinBacon.SetTracker(pigsNeeded);
        AT_CluckMe.SetTracker(chickensNeeded);
        AT_GlueFactory.SetTracker(horsesNeeded);
        AT_PublicMenace.SetTracker(npcsNeeded);
        AT_GraveDigger.SetTracker(gravesNeeded);
        AT_SpellingBee.SetTracker(lettersNeeded);
        AT_Colonel.SetTracker(chickenmenNeeded);
        AT_HayMan.SetTracker(scarecrowsNeeded);
        AT_FunnyGuy.SetTracker(jestersNeeded);
    }

    public void DisplayAchievment()
    {
        _achievementGameObject.transform.localScale = Vector3.zero;
        _achievementCanvas.enabled = true;
        _achievementGameObject.transform.DOScale(1f, 1f).SetEase(Ease.InOutElastic);
        StartCoroutine(EndDisplayAchievment());
    }

    IEnumerator EndDisplayAchievment()
    {
        yield return new WaitForSeconds(3);
        _achievementGameObject.transform.DOScale(0f, 1f).SetEase(Ease.OutFlash);
        yield return new WaitForSeconds(1);
        _achievementCanvas.enabled = false;    
        
    }

    private void DoAllTheTrackerStuff(int trackedVal, int maxVal, AchievementBoxDetail AT, string achvName, bool unlock)
    {
        if (trackedVal <= maxVal && unlock == false)
        {
            AT.UpdateTrackerText(trackedVal, maxVal);
        }
        if (trackedVal >= maxVal && unlock == false)
        {
            unlock = true;
            _achievementText.text = achvName;
            DisplayAchievment();
            AT._greenTick.SetActive(true);
            AT.CapTracker(maxVal);
           
        }
    }

    public void Pegasus()  
    {
        DoAllTheTrackerStuff(airTrickTracker, airTricksNeeded, AT_Pegasus, "Pegasus", unlockPegasus);
        //if (airTrickTracker >= airTricksNeeded && unlockPegasus == false)
        //{
        //    unlockPegasus = true;
        //    _achievementText.text = "Pegasus";
        //    DisplayAchievment();
        //    AT_Pegasus._greenTick.SetActive(true);
        //    AT_Pegasus.CapTracker(airTricksNeeded);
        //}
        //if (airTrickTracker <= airTricksNeeded && unlockPegasus == false)
        //{
        //    AT_Pegasus.UpdateTrackerText(airTrickTracker, airTricksNeeded);
        //}
    }
    public void SmoothCriminal() 
    { /* Remove max wanted level */

        _achievementText.text = "Smooth Criminal";
    }

    public void Collidesdale() 
    { /* Break 100 objects */
        
        if(stallTracker >= stallsNeeded && unlockCollidesdale == false)
        {
            unlockCollidesdale = true;
            _achievementText.text = "Collidesdale";
            DisplayAchievment();
            AT_Collidesdale._greenTick.SetActive(true);
            AT_Collidesdale.CapTracker(stallsNeeded);
        }
        if (stallTracker <= stallsNeeded && unlockCollidesdale == false)
        {   AT_Collidesdale.UpdateTrackerText(stallTracker, stallsNeeded); }
    }
    public void BaaBoom() 
    { /* Find the barrel sheep */
        unlockBaaBoom = true;
        _achievementText.text = "Baa Boom!";
        DisplayAchievment();
    }
    public void PloughHorse() 
    { /* Destroy (X) amount of fences */
        
        if(fenceTracker >= fencesNeeded && unlockPloughHorse == false)
        {
            unlockPloughHorse = true;
            _achievementText.text = "Plough Horse";
            DisplayAchievment();
            AT_PloughHorse._greenTick.SetActive(true);
            AT_PloughHorse.CapTracker(fencesNeeded);
        }
        if (fenceTracker <= fencesNeeded && unlockPloughHorse == false)
        {   AT_PloughHorse.UpdateTrackerText(fenceTracker, fencesNeeded); }
    }
    public void ShowPony() 
    { /* Change appearance */
        
        _achievementText.text = "Show Pony";
    }
    public void PlatniumDriver() 
    { /* Complete all quests */
        if (platniumTracker >= taxiNeeded && unlockPlatniumDriver == false)
        {
            unlockPlatniumDriver = true;
            _achievementText.text = "Platnium Driver";
            DisplayAchievment();
            AT_PlatniumDriver._greenTick.SetActive(true);
            AT_PlatniumDriver.CapTracker(taxiNeeded);
        }
        if (platniumTracker <= taxiNeeded && unlockPlatniumDriver == false)
        { AT_PlatniumDriver.UpdateTrackerText(platniumTracker, taxiNeeded); }
    }

    public void BBC()
    {
        if(unlockBBC == false)
        {
            unlockBBC = true;
            _achievementText.text = "B.B.C.";
            DisplayAchievment();
            AT_BBC._greenTick.SetActive(true);
        }
    }

    public void MakinBacon()
    { /* Pigs */
        if (pigTracker >= pigsNeeded && unlockMakinBacon == false)
        {
            unlockMakinBacon = true;
            _achievementText.text = "Makin' Bacon";
            DisplayAchievment();
        }
    }

    public void CluckMe()
    { /* Chickens */
        if (cluckTracker >= chickensNeeded && unlockCluckMe == false)
        {
            unlockCluckMe = true;
            _achievementText.text = "Cluck Me!";
            DisplayAchievment();
        }
    }

    public void GlueFactory()
    { /* Horses */
        if (glueTracker >= horsesNeeded && unlockGlueFactory == false)
        {
            unlockGlueFactory = true;
            _achievementText.text = "Glue Factory";
            DisplayAchievment();
        }
    }
    public void GraveDigger()
    { /* Graves */
        if (graveTracker >= gravesNeeded && unlockGraveDigger == false)
        {
            unlockGraveDigger = true;
            _achievementText.text = "Grave Digger";
            DisplayAchievment();
        }
    }

    public void Menace()
    { /* Graves */
        if (menaceTracker >= npcsNeeded && unlockMenace == false)
        {
            unlockMenace = true;
            _achievementText.text = "Menace";
            DisplayAchievment();
        }
    }


}
