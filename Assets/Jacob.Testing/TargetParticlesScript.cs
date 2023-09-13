using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetParticlesScript : MonoBehaviour
{
    public GameObject beamParticles;

    void LateUpdate()
    {
        if (PlayerData.isOccupied == true)
        {
            beamParticles.SetActive(true);
        }
        if (PlayerData.isOccupied == false)
        {
            beamParticles.SetActive (false);
        }
    }
}
