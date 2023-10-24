using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishonour : MonoBehaviour
{
    public static float dishonourLevel;
    
    [SerializeField] private int dishonourDepletionRate;
    public static int _dishonourDepletionRef;

    [Header("Set Dishonour Levels")]
    [SerializeField] private int oneStar;
    [SerializeField] private int twoStar;
    [SerializeField] private int threeStar;
    [SerializeField] private int fourStar;
    [SerializeField] private int fiveStar;

    [SerializeField] private int maxDishonourCap = 200;

    public static int _oneStar;
    public static int _twoStar;
    public static int _threeStar;
    public static int _fourStar;
    public static int _fiveStar;

    [Header("Dishonour GUI")]
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;
    [SerializeField] private GameObject star4;
    [SerializeField] private GameObject star5;

    [Header("Debug")]
    [SerializeField] private float startingDishonourLevel;
    [SerializeField] private float currentDishonourLevel;

    void Start()
    {
        dishonourLevel = startingDishonourLevel;
        _oneStar = oneStar;
        _twoStar = twoStar;
        _threeStar = threeStar;
        _fourStar = fourStar;
        _fiveStar = fiveStar;

        _dishonourDepletionRef = dishonourDepletionRate;
    }


    void Update()
    {
        DishonourUpdate();
        DishonourDepletion();
        StarGUI();      
    }

    private void DishonourDepletion()
    {
        if (dishonourLevel > 0) { dishonourLevel -= Time.deltaTime * dishonourDepletionRate; }
        else { dishonourLevel = 0;}
    }

    private void StarGUI()
    {
        if (dishonourLevel <_oneStar)
        {
            star1.SetActive(false); star2.SetActive(false); star3.SetActive(false); star4.SetActive(false); star5.SetActive(false);
        }
        if (dishonourLevel >=_oneStar)
        {
            star1.SetActive(true); star2.SetActive(false); star3.SetActive(false); star4.SetActive(false); star5.SetActive(false);
        }
        if (dishonourLevel >=_twoStar)
        {
            star2.SetActive(true); star3.SetActive(false); star4.SetActive(false); star5.SetActive(false);
        }
        if (dishonourLevel >=_threeStar)
        {
            star3.SetActive(true); star4.SetActive(false); star5.SetActive(false);
        }
        if (dishonourLevel >= _fourStar)
        {
            star4.SetActive(true); star5.SetActive(false);
        }
        if (dishonourLevel >= _fiveStar)
        {
            star5.SetActive(true);
        }
    }

    private void DishonourUpdate()
    {
        currentDishonourLevel = dishonourLevel;
        if(dishonourLevel >= maxDishonourCap)
        {
            dishonourLevel = maxDishonourCap;
        }
    }
}
