using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigateMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private List<GameObject> _tabs = new List<GameObject>();
    [SerializeField] private Canvas _canvas;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        Buttons();
    }

    private void Buttons()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(); 
        foreach(Transform child in allChildren)
        {
            if (child.GetComponent<CustomisationTab>() != null || child.GetComponent<Reset>() != null) _tabs.Add(child.gameObject);
        }
    }

    public void ShowCustomisation(bool show)
    {
        _canvas.gameObject.SetActive(show);
    }
}
