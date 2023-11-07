using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigateMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tabs = new List<GameObject>();

    private void Awake()
    {
        Buttons();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
