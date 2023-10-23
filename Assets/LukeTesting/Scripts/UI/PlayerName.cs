using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    private TMP_InputField _playerText;
    
   private void Awake()
   {
        _playerText = GetComponent<TMP_InputField>();
        _playerText.text = PlayerPrefs.GetString("PlayerName");
    }

    public void SaveName()
    {
        _playerName.text = _playerText.text;
        PlayerPrefs.SetString("PlayerName", _playerName.text);
        PlayerPrefs.Save();
    }

    public void ResetName()
    {
        PlayerPrefs.DeleteKey("PlayerName");
        _playerText.text = "";
    }
}
