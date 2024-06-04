using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoliceAI))]
public class EnemyKnockback : Knockback
{
    PoliceAI m_AI;

    protected override void Init()
    {
        base.Init();
        m_AI = GetComponent<PoliceAI>();
    }

    public override void KnockBack(Vector3 _dir, Vector3 _origin, float _objectMulti = 1)
    {
        base.KnockBack(_dir, _origin, _objectMulti);

        //m_AI.Stun();
    }
}
