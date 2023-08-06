using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishonour : MonoBehaviour
{
    public static float dishonourLevel;
    
    [SerializeField] private int dishonourDepletionRate;

    [Header("Dishonour Values")]
    [SerializeField] private int lowPriorityValue;
    [SerializeField] private int medPriorityValue;
    [SerializeField] private int highPriorityValue;

    [Header("Set Dishonour Levels")]
    [SerializeField] private int oneStar;
    [SerializeField] private int twoStar;
    [SerializeField] private int threeStar;
    public static int _oneStar;
    public static int _twoStar;
    public static int _threeStar;

    [Header("Dishonour GUI")]
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;




    [Header("Debug")]
    [SerializeField] private float startingDishonourLevel;
    [SerializeField] private float currentDishonourLevel;

    void Start()
    {
        dishonourLevel = startingDishonourLevel;
        _oneStar = oneStar;
        _twoStar = twoStar;
        _threeStar = threeStar;
    }


    void Update()
    {
        currentDishonourLevel = dishonourLevel;
        DishonourDepletion();
        StarGUI();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LowPriority"))
        {
            dishonourLevel = dishonourLevel + lowPriorityValue;
            Debug.Log("LowPriority");
        }
        if (other.CompareTag("MedPriority"))
        {
            dishonourLevel = dishonourLevel + medPriorityValue;
            Debug.Log("MedPriority");
        }
        if (other.CompareTag("HighPriority"))
        {
            dishonourLevel = dishonourLevel + highPriorityValue;
            Debug.Log("HighPriority");
        }
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
            star1.SetActive(false); star2.SetActive(false); star3.SetActive(false);
        }
        if (dishonourLevel >=_oneStar)
        {
            star1.SetActive(true); star2.SetActive(false); star3.SetActive(false);
        }
        if (dishonourLevel >=_twoStar)
        {
            star2.SetActive(true); star3.SetActive(false);
        }
        if (dishonourLevel >=_threeStar)
        {
            star3.SetActive(true);
        }
    }


}
