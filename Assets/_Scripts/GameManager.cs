using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    void CreateSingleton()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }
    #endregion

    [SerializeField] int m_gold;
    public int gold { get { return m_gold; } }
    public void SetGold(int _goldAmount)
    {
        int oldVal = m_gold;
        m_gold = _goldAmount;

        onGoldChanged?.Invoke(oldVal, _goldAmount);
    }

    public void AddGold(int _goldToAdd)
    {
        int oldVal = m_gold;
        m_gold += _goldToAdd;

        onGoldChanged?.Invoke(oldVal, m_gold);
    }

    private void OnValidate() => onGoldChanged?.Invoke(m_gold, m_gold);

    public Action<int, int> onGoldChanged;

    public Color m_affordColour, m_notAffordColour;

    private void Awake()
    {
        CreateSingleton();

        onGoldChanged?.Invoke(m_gold, m_gold);

        InputUser.onUnpairedDeviceUsed += UnpairedDeviceUsed;
    }

    public void UnpairedDeviceUsed(InputControl _inputControl, InputEventPtr _inputEventPtr)
    {
       foreach (PlayerInputHandler playerInput in FindObjectsOfType<PlayerInputHandler>())
        {
            playerInput.UnpairedDeviceUsed(_inputControl, _inputEventPtr);
        }
    }
}
