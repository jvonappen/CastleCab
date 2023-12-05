using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBC : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            AchievementManager.Instance.BBC();
        }
    }
}
