using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AchievementLocationTrigger : MonoBehaviour
{
    [SerializeField] SO_Achievement m_achievementToTrigger;

    private void Awake()
    {
        if (!GetComponent<Collider>().isTrigger) 
        {
            Debug.LogWarning("Game object: '" + gameObject + "' has a 'AchievementLocationTrigger' component, but does not contain a trigger collider. Please either remove the component or enable 'isTrigger' on the collider.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!AchievementManager.Instance.IsAchievementCompleted(m_achievementToTrigger))
            {
                AchievementManager.Instance.CompleteAchievement(m_achievementToTrigger);
            }
        }
    }
}
