using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int scoreValue = 0;

    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _startValue = 0;

    private void Awake()
    {
        scoreValue = _startValue;
    }
    void Start()
    {
        _scoreText.text = scoreValue.ToString();
    }

    private void FixedUpdate()
    {
        _scoreText.text = scoreValue.ToString();
    }
          
}
