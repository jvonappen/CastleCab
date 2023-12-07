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
    [SerializeField] private AchievementBoxDetail AT_SmoothCriminal;
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
    [Header("SpaDay")]
    [SerializeField] private AchievementBoxDetail AT_SpaDay;
    private bool unlockSpaDay = false;
    [Space]
    [Header("PartyGoblins")]
    [SerializeField] private AchievementBoxDetail AT_PartyGoblins;
    private bool unlockPartyGoblins = false;
    [Header("OldSpice")]
    [SerializeField] private AchievementBoxDetail AT_OldSpice;
    private bool unlockOldSpice = false;
    [Header("TheColonel")]
    [SerializeField] private AchievementBoxDetail AT_Colonel;
    private bool unlockColonel = false;
    [Space]
    [Header("HayMan")]
    [SerializeField] private AchievementBoxDetail AT_HayMan;
    private bool unlockHayMan = false;
    [Space]
    [Header("FunnyGuy")]
    [SerializeField] private AchievementBoxDetail AT_FunnyGuy;
    private bool unlockFunnyGuy = false;
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

    private void DoAllTheTrackerStuff(int trackedVal, int maxVal, AchievementBoxDetail AT, string achvName, ref bool unlock)
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

    private void DoOnceOffStuff(ref bool unlock, string achvText, AchievementBoxDetail AT)
    {
        if (unlock == false)
        {
            unlock = true;
            _achievementText.text = achvText;
            DisplayAchievment();
            AT._greenTick.SetActive(true);
        }
    }

    public void Pegasus()  
    {
        DoAllTheTrackerStuff(airTrickTracker, airTricksNeeded, AT_Pegasus, "Pegasus", ref unlockPegasus);
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
    {
        DoOnceOffStuff(ref unlockBaaBoom, "Baa Boom!", AT_BaBoom);
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
        DoOnceOffStuff(ref unlockBBC, "B.B.C.", AT_BBC);
    }

    public void SpaDay()
    {
        DoOnceOffStuff(ref unlockSpaDay, "Spa Day", AT_SpaDay);
    }

    public void PartyGoblin()
    {
        DoOnceOffStuff(ref unlockPartyGoblins, "Party Goblins", AT_PartyGoblins);
    }
    public void OldSpice()
    {
        DoOnceOffStuff(ref unlockOldSpice, "Old Spice", AT_OldSpice);
    }

    public void MakinBacon()
    {
        DoAllTheTrackerStuff(pigTracker, pigsNeeded, AT_MakinBacon, "Makin' Bacon", ref unlockMakinBacon);
    }

    public void CluckMe()
    {
        DoAllTheTrackerStuff(cluckTracker, chickensNeeded, AT_CluckMe, "Cluck Me!", ref unlockCluckMe);
    }

    public void GlueFactory()
    {
        DoAllTheTrackerStuff(glueTracker, horsesNeeded, AT_GlueFactory, "Glue Factory", ref unlockGlueFactory);
    }
    public void GraveDigger()
    {
        DoAllTheTrackerStuff(graveTracker, gravesNeeded, AT_GraveDigger, "Grave Digger", ref unlockGraveDigger);
    }

    public void PublicMenace()
    {
        DoAllTheTrackerStuff(menaceTracker, npcsNeeded, AT_PublicMenace, "Public Menace", ref unlockMenace);
    }

    public void SpellingBee()
    {
        DoAllTheTrackerStuff(spellingTracker, lettersNeeded, AT_SpellingBee, "Spelling Bee", ref unlockSpellingBee);
    }

    public void Colonel()
    {
        DoOnceOffStuff(ref unlockColonel, "The Colonel", AT_Colonel);
    }
    public void HayMan()
    {
        DoOnceOffStuff(ref unlockHayMan, "Hay-Man!", AT_HayMan);
    }
    public void FunnyGuy()
    {
        DoOnceOffStuff(ref unlockFunnyGuy, "Funny Guy", AT_FunnyGuy);
    }

}
