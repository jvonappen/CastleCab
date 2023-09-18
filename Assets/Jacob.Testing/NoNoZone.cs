using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoNoZone : MonoBehaviour
{
    private static bool _failed = false;
    [SerializeField] private GameObject failedInGUI;

    private void Start()
    {
        if(_failed == true)
        {
            AudioManager.Instance.PlaySFX("NoNoZone");
            failedInGUI.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       _failed= true;
        SceneManager.LoadScene(0);
    }
}
