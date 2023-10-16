using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentToggle : MonoBehaviour
{
    public static AchievmentToggle Instance;

    [SerializeField] private Canvas _achivmentCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _achivmentCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayAchievment()
    {
        _achivmentCanvas.enabled = true;
    }

    public void HideAchievment()
    {
        _achivmentCanvas.enabled = false;
    }
}
