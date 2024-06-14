using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent<Interact> onInteract, onEnterRange, onExitRange;

    public void Interact(Interact _interactor) => onInteract?.Invoke(_interactor);
    public void OnEnterInteractable(Interact _interactor) => onEnterRange?.Invoke(_interactor);
    public void OnExitInteractable(Interact _interactor) => onExitRange?.Invoke(_interactor);
}
