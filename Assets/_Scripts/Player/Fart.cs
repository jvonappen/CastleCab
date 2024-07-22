using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class Fart : MonoBehaviour
{
    [SerializeField] ParticleSystem m_fart, m_megaFart;

    [Header("Cooldown")]
    [SerializeField] int m_fartAmountBuffer = 10;
    [SerializeField] float m_fartAmountAddTime = 12, m_fartCooldown = 0.3f;

    bool m_onCooldown = false;

    PlayerInputHandler m_inputHandler;
    private void Awake() => m_inputHandler = GetComponent<PlayerInputHandler>();

    private void OnEnable()
    {
        m_inputHandler.m_playerControls.Controls.Fart.performed += NormalFart;
        m_inputHandler.m_playerControls.Controls.MegaFart.performed += MegaFart;
    }

    private void OnDisable()
    {
        m_inputHandler.m_playerControls.Controls.Fart.performed -= NormalFart;
        m_inputHandler.m_playerControls.Controls.MegaFart.performed -= MegaFart;
    }

    void NormalFart(InputAction.CallbackContext context) => NormalFart();
    void MegaFart(InputAction.CallbackContext context) => MegaFart();

    public void NormalFart()
    {
        if (m_fartAmountBuffer > 0 && !m_onCooldown)
        {
            m_onCooldown = true;
            m_fartAmountBuffer--;

            m_fart.gameObject.SetActive(true);
            m_fart.Play();
            AudioManager.Instance.PlaySoundAtLocation("FartsShort", transform.GetChild(0).position);

            TimerManager.RunAfterTime(() => m_onCooldown = false, m_fartCooldown);
            TimerManager.RunAfterTime(() => m_fartAmountBuffer++, m_fartAmountAddTime);
        }
    }

    public void MegaFart()
    {
        if (m_fartAmountBuffer > 0 && !m_onCooldown)
        {
            m_onCooldown = true;
            m_fartAmountBuffer--;

            m_megaFart.gameObject.SetActive(true);
            m_megaFart.Play();
            AudioManager.Instance.PlaySoundAtLocation("FartsLong", transform.GetChild(0).position);

            TimerManager.RunAfterTime(() => m_onCooldown = false, m_fartCooldown);
            TimerManager.RunAfterTime(() => m_fartAmountBuffer++, m_fartAmountAddTime);
        }
    }
}
