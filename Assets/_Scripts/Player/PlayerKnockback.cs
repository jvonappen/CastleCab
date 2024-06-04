using UnityEngine;

public class PlayerKnockback : Knockback
{
    PlayerMovement m_player;
    public PlayerMovement playerMovement { get { return m_player; } }
    protected override void Init()
    {
        base.Init();
        m_player = GetComponentInParent<PlayerMovement>();
    }

    public override void KnockBack(Vector3 _dir, Vector3 _origin, float _objectMulti = 1)
    {
        if (m_player.isHurricane) return;
        
        base.KnockBack(_dir, _origin, _objectMulti);

        m_player.isSmackStunned = true;

        // Failsafe for if player doesn't leave ground. Unstuns player after 1 second if so
        TimerManager.RunAfterTime(() =>
        {
            if (m_player.isGrounded && m_player.isSmackStunned)
            {
                m_player.isSmackStunned = false;
                m_player.SetCurrentSpeed(m_player.currentSpeed / 2);
            }
        }
        , 1f);
    }
}
