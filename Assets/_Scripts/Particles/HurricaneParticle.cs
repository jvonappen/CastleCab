using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneParticle : ParticleHandler
{

    private void OnEnable()
    {
        m_playerMovement.onHurricane += PlayParticle; //OnHurricane;
        m_playerMovement.onHurricaneCanceled += StopParticle;// OnHurricaneCanceled;
    }

    private void OnDisable()
    {
        m_playerMovement.onHurricane -= PlayParticle; //OnHurricane;
        m_playerMovement.onHurricaneCanceled -= StopParticle;// OnHurricaneCanceled;
    }

    //void OnHurricane()
    //{
    //    PlayParticle();
    //}
    //
    //void OnHurricaneCanceled()
    //{
    //    StopParticle();
    //}
}
