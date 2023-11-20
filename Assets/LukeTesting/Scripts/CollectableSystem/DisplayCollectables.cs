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
        //Create game object for displaying collectable on canvas
        foreach(char character in _characters)
        {
            //Create text box and assign to canvas
            GameObject letter =  Instantiate(_textBox); 
            letter.gameObject.name = character.ToString();
            _collectables.Add(letter); 
            letter.transform.SetParent(this.transform); 

            //Set positon of each text box and update positions
            RectTransform letterRect = letter.GetComponent<RectTransform>(); 
            letterRect.localPosition = new Vector2(_spacingX, _spacingY); 
            _spacingY -= _adjustYSpacing; 
            _spacingX += _adjustXSpacing;

            //Set text component of Text box game object
            TextMeshProUGUI collectableText = letter.GetComponent<TextMeshProUGUI>();
            collectableText.text = character.ToString();
        }
    }

    public void SetCollectableActive(Collectable collectable)
    {
        //Display text box on collection
        for (int i = 0; i < _collectables.Count; i++)
        {
            if (i == collectable._letterPos)
            {
                _collectables[i].SetActive(true);  
            }
        }
    }
}
