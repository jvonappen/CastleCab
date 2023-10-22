using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    [SerializeField] private Button _resetButton;
    [SerializeField] private List<GameObject> _tabs;

    private void Awake()
    {
        _resetButton.onClick.AddListener(OnResetButtonClicked);
        GetTabs();
    }

    private void GetTabs()
    {
        for (int i = 0; i < 12; i++)
        {
            Debug.Log(transform.parent.GetChild(i).gameObject.name);
            if (transform.parent.GetChild(i).gameObject.GetComponent<CustomisationTab>())
            {
                _tabs.Add(transform.parent.GetChild(i).gameObject);
            }
        }
    }

    private void OnResetButtonClicked()
    {
        PlayerPrefs.DeleteAll();
    }
}
