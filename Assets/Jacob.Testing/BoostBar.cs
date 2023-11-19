using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField] private Image boostFill;
    [SerializeField] private float depletionRate = 0.1f;
    [SerializeField] private float refillrate = 0.1f;

    public static bool canBoost;


   // Start is called before the first frame update
   void Start()
    {
        boostFill.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovement.isBoostingForBoostBar == true)
        {
            boostFill.fillAmount = boostFill.fillAmount - depletionRate * Time.deltaTime;
        }
        else if (PlayerMovement.isBoostingForBoostBar == false)
        {
            boostFill.fillAmount = boostFill.fillAmount + refillrate * Time.deltaTime;
        }
        CanBoostCheck();
    }

    private void CanBoostCheck()
    {
        if(boostFill.fillAmount > 0)
        {
            canBoost = true;
        }
        else
        { canBoost = false;  Debug.Log("Can't Boost rn"); }
    }
}
