using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBC : MonoBehaviour
{
    [SerializeField] private bool _BBC = false;
    [SerializeField] private bool _spaDay = false;
    [SerializeField] private bool _partyGoblins = false;
    [SerializeField] private bool _oldSpice = false;
    [SerializeField] private bool _theColonel = false;
    [SerializeField] private bool _hayMan = false;
    [SerializeField] private bool _funnyGuy = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && _BBC == true)
        {
            AchievementManagerOld.Instance.BBC();
        }
        if (other.gameObject.tag == "Player" && _spaDay == true)
        {
            AchievementManagerOld.Instance.SpaDay();
        }
        if(other.gameObject.tag == "Player" && _partyGoblins == true)
        {
            AchievementManagerOld.Instance.PartyGoblin();
        }
        if(other.gameObject.tag == "Player" && _oldSpice == true)
        {
            AchievementManagerOld.Instance.OldSpice();
        }
        if(other.gameObject.tag == "Player" && _theColonel == true)
        {
            AchievementManagerOld.Instance.Colonel();
        }
        if(other.gameObject.tag == "Player" && _hayMan == true)
        {
            AchievementManagerOld.Instance.HayMan();
        }
        if(other.gameObject.tag == "Player" && _funnyGuy == true)
        {
            AchievementManagerOld.Instance.FunnyGuy();
        }
    }
}
