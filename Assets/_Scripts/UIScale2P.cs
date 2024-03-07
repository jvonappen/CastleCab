using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIScale2P : MonoBehaviour
{
    public void OnPlayerJoined(PlayerInput _player, List<PlayerInput> _players)
    {
        if (_players.Count == 2) transform.localScale = Vector3.one * 0.5f;
        else transform.localScale = Vector3.one;
    }
}
