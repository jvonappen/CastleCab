using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DisplayCollectables : MonoBehaviour
{
    [SerializeField] private List<char> _characters;
    [SerializeField] private List<GameObject> _collectables;
    [SerializeField] private GameObject _textBox;
    [SerializeField] private int _spacingX = -360;
    [SerializeField] private int _spacingY = 150;
    [SerializeField] private int _adjustYSpacing = 50;
    [SerializeField] private int _adjustXSpacing = 0;

    private void Awake()
    {
        AddLetter();
    }

    private void AddLetter()
    {
        foreach(char character in _characters)
        {
            GameObject letter =  Instantiate(_textBox);
            letter.gameObject.name = character.ToString();
            _collectables.Add(letter);
            letter.transform.SetParent(this.transform);
            RectTransform letterRect = letter.GetComponent<RectTransform>();
            letterRect.localPosition = new Vector2(_spacingX, _spacingY);
            _spacingY -= _adjustYSpacing;
            _spacingX += _adjustXSpacing;
            TextMeshProUGUI collectableText = letter.GetComponent<TextMeshProUGUI>();
            collectableText.text = character.ToString();
        }
    }

    public void SetCollectableActive(Collectable collectable)
    {
        foreach(GameObject collectables in _collectables)
        {
            if (collectable._collectibleLetter.ToString() == collectables.gameObject.name)
            {
                collectables.SetActive(true);
            }
        }
    }
}
