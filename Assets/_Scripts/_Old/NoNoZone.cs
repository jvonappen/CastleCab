using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoNoZone : MonoBehaviour
{
    public static bool _failed = false;
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
        if(other.tag == "Player")
        {
            _failed = true;
            SceneManager.LoadScene(0);
        }
        if (other.tag != "Player")
        {
            Debug.Log("Something touched the NoNo Square!");
        }


    }
}
