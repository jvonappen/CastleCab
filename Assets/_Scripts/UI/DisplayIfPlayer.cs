using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayIfPlayer : MonoBehaviour
{
    [SerializeField] PlayerInput m_playerInput;
    [SerializeField] List<int> m_usersToDisplay;

    public void Start()
    {
        if (m_playerInput)
        {
            if (m_usersToDisplay.Contains(m_playerInput.user.index + 1)) gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
        else Debug.LogWarning("No player input reference on object: " + gameObject);
    }
}
