using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    [SerializeField] private Button _resetButton;
    [SerializeField] private List<GameObject> _tabs;
    [SerializeField] private PlayerName _playerName;

    private void Awake()
    {
        _resetButton.onClick.AddListener(OnResetButtonClicked);
        _playerName = FindObjectOfType<PlayerName>();
        GetTabs();
    }

    private void GetTabs()
    {
        for (int i = 0; i < 12; i++)
        {
            if (transform.parent.GetChild(i).gameObject.GetComponent<CustomisationTab>())
            {
                _tabs.Add(transform.parent.GetChild(i).gameObject);
            }
        }
    }

    private void OnResetButtonClicked()
    {
        foreach (GameObject tab in _tabs)
        {
            tab.GetComponent<CustomisationTab>().ResetCart();
        }
        _playerName.ResetName();
    }
}
