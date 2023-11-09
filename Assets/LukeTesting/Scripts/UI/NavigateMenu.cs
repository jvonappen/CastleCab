using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigateMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private List<GameObject> _tabs = new List<GameObject>();
    [SerializeField] private EnterCustomisation _enterCustomisation;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        Buttons();
    }

    private void Update()
    {

    }

    private void Buttons()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(); 
        foreach(Transform child in allChildren)
        {
            if (child.GetComponent<CustomisationTab>() != null || child.GetComponent<Reset>() != null) _tabs.Add(child.gameObject);
        }
    }
}
