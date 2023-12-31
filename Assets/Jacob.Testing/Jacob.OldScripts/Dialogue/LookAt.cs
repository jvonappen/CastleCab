using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Debug-Testing")]
    [SerializeField] private float X;
    [SerializeField] private float Y;
    [SerializeField] private float Z;

    [SerializeField] private GameObject villager;

    private void Awake()
    {
        villager = this.gameObject;
    }
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.LookAt(player.transform);
        transform.Rotate(X,Y,Z);
    }
}
