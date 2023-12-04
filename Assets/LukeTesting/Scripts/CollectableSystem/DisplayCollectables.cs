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
    [SerializeField] private float _spacingX = 0;
    [SerializeField] private int _spacingY = 145;
    [SerializeField] private int _adjustYSpacing = 0;
    [SerializeField] private int _adjustXSpacing = 90;
    [SerializeField] private Color _uncollectedColour;
    [SerializeField] private Color _collectedColour;
    private Tween _tween;

    private void Awake()
    {
        this.GetComponent<CanvasGroup>().alpha = 0;
        _spacingX = CenterCollectableLetters();
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
            collectableText.color = _uncollectedColour;
        }
    }

    public void SetCollectableActive(Collectable collectable)
    {
        //Display text box on collection
        for (int i = 0; i < _collectables.Count; i++)
        {
            if (i == collectable._letterPos)
            {
                this.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
                _collectables[i].GetComponent<TextMeshProUGUI>().color = _collectedColour;
                Coroutine letter = StartCoroutine(CollectableTween(_collectables[i]));
            }
        }
    }

    private bool CompleteCheck()
    {
        //Display text box on collection
        for (int i = 0; i < _collectables.Count; i++)
        {
            if (_collectables[i].GetComponent<TextMeshProUGUI>().color != _collectedColour)
            {
                return false;
            }
        }
        return true;
    }

    private float CenterCollectableLetters()
    {
        float textBoxWidth = _textBox.GetComponent<RectTransform>().sizeDelta.x;
        float letterGap = _adjustXSpacing - textBoxWidth;
        float wordLength = (_characters.Count * textBoxWidth) + (letterGap * (_characters.Count - 1));
        return (0 - (wordLength / 2) + (textBoxWidth /2));
    }

    IEnumerator CollectableTween(GameObject collectable)
    {
        yield return new WaitForEndOfFrame();
        _tween = collectable.transform.DOScale(2f, 0.8f).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(1f);
        _tween = collectable.transform.DOScale(1f, 0.5f).SetEase(Ease.InSine);
        
        if (CompleteCheck()) //finial special tween when all letters are collected
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < _collectables.Count; i++)
            {
                _collectables[i].transform.DOPunchScale(Vector3.one, 1f);
            }
            yield return new WaitForSeconds(2f);
        }
        else yield return new WaitForSeconds(1f);

        this.GetComponent<CanvasGroup>().DOFade(0, 1f);
        yield return null;
    }
}
