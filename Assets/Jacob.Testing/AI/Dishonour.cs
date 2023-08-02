using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishonour : MonoBehaviour
{
    public static float dishonourLevel;
    [SerializeField] private float startingDishonourLevel;
    [SerializeField] private float dishonourDepletionRate;

    [Header("Dishonour Values")]
    [SerializeField] private float lowPriorityValue;
    [SerializeField] private float medPriorityValue;
    [SerializeField] private float highPriorityValue;

    [Header("Set Dishonour Levels")]
    [SerializeField] private int oneStar;
    [SerializeField] private int twoStar;
    [SerializeField] private int threeStar;
    public static float _oneStar;
    public static float _twoStar;
    public static float _threeStar;


    [Header("Debug")]
    [SerializeField] private float currentDishonourLevel;

    void Start()
    {
        dishonourLevel = startingDishonourLevel;
    }


    void Update()
    {
        currentDishonourLevel = dishonourLevel;

        dishonourLevel -= Time.deltaTime * dishonourDepletionRate;
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

}
