using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class Fart : MonoBehaviour
{
    [SerializeField] ParticleSystem m_fart, m_megaFart;

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
        m_fart.gameObject.SetActive(true);
        m_fart.Play();
        AudioManager.Instance.PlayGroupAudio("FartsShort");
    }

    public void MegaFart()
    {
        m_megaFart.gameObject.SetActive(true);
        m_megaFart.Play();
        AudioManager.Instance.PlayGroupAudio("FartsLong");
    }
}
