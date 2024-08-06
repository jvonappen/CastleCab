using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;

    [Header("MiniMap Icons")]
    [SerializeField] public GameObject targetMarkerP1;
    [SerializeField] public GameObject targetMarkerP2;
    [SerializeField] public GameObject targetMarkerP3;
    [SerializeField] public GameObject targetMarkerP4;

    [Header("Beams")]
    [SerializeField] public GameObject beamP1;
    [SerializeField] public GameObject beamP2;
    [SerializeField] public GameObject beamP3;
    [SerializeField] public GameObject beamP4;

    [Header("Delivery Zones")]
    [SerializeField] public GameObject[] zone1DestinationsValley;
    [SerializeField] public GameObject[] zone2DestinationsDock;
    [SerializeField] public GameObject[] zone3DestinationsGraveyard;
    [SerializeField] public GameObject[] zone4DestinationsTown;
    [SerializeField] public GameObject[] zone5DestinationsHilltop;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    gameObject.transform.SetParent(null);
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        Instance = this;
    }
}
