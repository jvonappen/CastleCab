using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int scoreValue = 0;

    [SerializeField] private List<TextMeshProUGUI> _scoreDisplays;
    private int _startValue = 0;

    private void Awake() => scoreValue = _startValue;
    void Start() => UpdateDisplay();
    private void Update() => UpdateDisplay();
    
    void UpdateDisplay()
    {
        foreach (TextMeshProUGUI display in _scoreDisplays)
        {
            display.text = scoreValue.ToString();
        }
    }
}
