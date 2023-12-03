using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRandom : MonoBehaviour
{
    [Header("Left Hand")]
    [SerializeField] private GameObject[] _leftHand;
    [Space]
    [Header("Right Hand")]
    [SerializeField] private GameObject[] _rightHand;

    void Awake()
    {
        int leftVal = UnityEngine.Random.Range(0, _leftHand.Length);
        _leftHand[leftVal].SetActive(true);

        int rightVal = UnityEngine.Random.Range(0, _rightHand.Length);
        _rightHand[rightVal].SetActive(true);

    }

}
