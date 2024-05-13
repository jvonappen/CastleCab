using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayerCartColour : MonoBehaviour
{
    [SerializeField] private MeshRenderer cartMesh;
    [SerializeField] private Material[] cartColour;

    private int thisPlayerNumber;

    private void Awake()
    {
        
        thisPlayerNumber = WagonData.playerNumber;
        cartMesh.material = cartColour[thisPlayerNumber];
    }
}
