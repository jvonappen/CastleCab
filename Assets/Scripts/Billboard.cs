using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //Camera mainCamera;

    [SerializeField] private GameObject _cmCam;
    void Start()
    {
        //mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        transform.LookAt(_cmCam.transform);      
        transform.Rotate(0, 180, 0);
    }
}
