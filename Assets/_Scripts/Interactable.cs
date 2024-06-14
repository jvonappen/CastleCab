using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Action onInteract;

    public void Interact(Interact _interactor)
    {
        onInteract?.Invoke();
        Debug.Log(_interactor + " interacted");
    }
}
