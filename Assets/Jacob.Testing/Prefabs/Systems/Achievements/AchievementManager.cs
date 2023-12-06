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
     * Elderly Citizen’s Home – Delivery grandma 
     * SmoothCriminal - Remove max wanted level 
  X   * Collidesdale - Break 100 Market Stalls
  X   * BaaBoom 
  X   * PloughHorse – Fences
  X   * Pegasus - Do a flip
     * ShowPony – Colour change
  X   * Platnium Driver – Complete(x) Quests
  X   * BBC – Find the big big chook
    
 Collectables: 
     * Letters [Castle Cab] **Castle Cab
     * Chicken Men ** The Colonel
     * Scarecrows ** Hay-man
     * Jesters  ** Funny Guy

     * Pigs ** Makin' Bacon
     * Chickens ** Cluck Me!
     * Horses ** Glue factory
     * NPCS ** Menace
     * Tomb stones ** Grave Digger

     */

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _achievementGameObject;

    public static AchievementManager Instance;

    [SerializeField] private Canvas _achievementCanvas;
    [SerializeField] private TextMeshProUGUI _achievementText;

    [Header("Pegasus")]
    public bool unlockPegasus = false;
    public static int airTrickTracker = 0;
    [SerializeField] private int airTricksNeeded = 5;
    [SerializeField] private AchievementBoxDetail AT_Pegasus;

    [Header("SmoothCriminal")]
    public bool unlockSmoothCriminal = false;

    [Header("BaconEggs")]
    public bool unlockBaconEggs = false;
    private float bTimer = 5;
    public static bool eggCheck = false;
    public static bool baconCheck = false;

    [Header("Collidesdale")]
    public bool unlockCollidesdale = false;
    public static int stallTracker = 0;
    [SerializeField] private int stallsNeeded = 20;
    [SerializeField] private AchievementBoxDetail AT_Collidesdale;

    [Header("BaaBoom")]
    public static bool unlockBaaBoom = false;

    [Header("PloughHorse")]
    public bool unlockPloughHorse = false;
    public static int fenceTracker = 0;
    [SerializeField] private int fencesNeeded = 50;
    [SerializeField] private AchievementBoxDetail AT_PloughHorse;

    [Header("ShowPony")]
    public bool unlockShowPony = false;

    [Header("BBC")]
    public bool unlockBBC = false;

    [Header("PlatniumDriver")]
    public bool unlockPlatniumDriver = false;
    public static int platniumTracker = 0;
    [SerializeField] private int taxiNeeded = 10;

    [Header("Makin'Bacon")]
    public bool unlockMakinBacon = false;
    public static int pigTracker = 0;
    [SerializeField] private int pigsNeeded = 10;

    [Header("CluckMe")]
    public bool unlockCluckMe = false;
    public static int cluckTracker = 0;
    [SerializeField] private int chickensNeeded = 10;

    [Header("GlueFactory")]
    public bool unlockGlueFactory = false;
    public static int glueTracker = 0;
    [SerializeField] private int horsesNeeded = 10;

    [Header("Menace")]
    public bool unlockMenace = false;
    public static int menaceTracker = 0;
    [SerializeField] private int npcsNeeded = 10;

    [Header("GraveDigger")]
    public bool unlockGraveDigger = false;
    public static int graveTracker = 0;
    [SerializeField] private int gravesNeeded = 10;



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

    public void Pegasus()  
    { /* Airtime of (5) seconds */
        if (airTrickTracker >= airTricksNeeded && unlockPegasus == false)
        {
            unlockPegasus = true;
            _achievementText.text = "Pegasus";
            DisplayAchievment();
            AT_Pegasus._greenTick.SetActive(true);
            AT_Pegasus.CapTracker(airTricksNeeded);
        }
        if (airTrickTracker <= airTricksNeeded && unlockPegasus == false)
        {
            AT_Pegasus.UpdateTrackerText(airTrickTracker, airTricksNeeded);
        }
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
        }
    }

    public void BBC()
    {
        if(unlockBBC == false)
        {
            unlockBBC = true;
            _achievementText.text = "B.B.C.";
            DisplayAchievment();
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
