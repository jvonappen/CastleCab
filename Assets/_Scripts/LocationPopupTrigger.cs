using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPopupTrigger : MonoBehaviour
{
    [SerializeField] string m_locationName;

    List<LocationPopup> m_playersInRange = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.tag == "Player")
        {
            LocationPopup popup = other.attachedRigidbody.GetComponentInParent<LocationPopup>();
            if (popup && !m_playersInRange.Contains(popup))
            {
                m_playersInRange.Add(popup);

                popup.Display(m_locationName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.tag == "Player")
        {
            LocationPopup popup = other.attachedRigidbody.GetComponentInParent<LocationPopup>();
            if (popup && m_playersInRange.Contains(popup))
            {
                m_playersInRange.Remove(popup);
            }
        }
    }
}
