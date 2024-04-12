using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTwin : MonoBehaviour
{
    [SerializeField] PlayerUpgrades m_playerUpgrades;

    [SerializeField] AttributePointDisplay m_pointDisplay;
    [SerializeField] CartUpgradeProgress m_thisHealth, m_thisStamina, m_thisSpeed, m_thisAttack;
    //[Space(10)]
    //[SerializeField] CartUpgradeProgress m_twinHealth;
    //[SerializeField] CartUpgradeProgress m_twinStamina, m_twinSpeed, m_twinAttack;

    public void SyncUpgrades()
    {
        //m_twinHealth.SetProgress(m_thisHealth.progress);
        //m_twinStamina.SetProgress(m_thisStamina.progress);
        //m_twinSpeed.SetProgress(m_thisSpeed.progress);
        //m_twinAttack.SetProgress(m_thisAttack.progress);

        m_pointDisplay.UpdateDisplay();

    }
}
