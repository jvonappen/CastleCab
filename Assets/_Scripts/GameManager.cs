using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    [SerializeField] List<GameObject> m_players;
    public List<GameObject> players { get { return m_players; } }
    public void AddPlayer(GameObject _player) => m_players.Add(_player);

    #region Gold
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

    

    public Action<int, int> onGoldChanged;
    #endregion

    public Color m_affordColour, m_notAffordColour;

    private void OnValidate()
    {
        onGoldChanged?.Invoke(m_gold, m_gold);
    }

    private void Awake()
    {
        CreateSingleton();

        onGoldChanged?.Invoke(m_gold, m_gold);
    }

    public void LoadScene(string _sceneName) => SceneManager.LoadScene(_sceneName);

    public void OpenCustomization()
    {
        foreach (GameObject player in players) player.GetComponent<CustomizationSpawner>().StartCustomization();
        InputManager.EnableSplitscreen();
    }
}
