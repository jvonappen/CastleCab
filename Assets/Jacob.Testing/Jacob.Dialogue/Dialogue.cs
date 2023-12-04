using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string[] _sentences;
    [SerializeField] private float _textSpeed;
    private int index;
    [Space]
    public AudioClip[] voices;
    public AudioClip[] punctuations;
    [Space]
    public AudioSource voiceSource;
    public AudioSource punctuationSource;

    [SerializeField] private PlayerInput _playerInput;


    // Start is called before the first frame update
    void Start()
    {
        _text.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerInput._playerControls.Controls.Interact.WasPressedThisFrame() /*Input.GetKeyUp(KeyCode.T*/)
        { 
            if(_text.text == _sentences[index]) { NextSentence(); }
            else { StopAllCoroutines(); _text.text = _sentences[index]; }
        }

    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        //type each character out one at a time

        foreach(char c in _sentences[index].ToCharArray())
        {
            _text.text += c;

            if (char.IsPunctuation(c) && !punctuationSource.isPlaying)
            {
                voiceSource.Stop();
                punctuationSource.clip = punctuations[Random.Range(0, punctuations.Length)];
                punctuationSource.Play();
            }
            if (char.IsLetter(c) && !voiceSource.isPlaying)
            {
                punctuationSource.Stop();
                voiceSource.clip = voices[Random.Range(0, voices.Length)];
                voiceSource.Play();
            }

            yield return new WaitForSeconds(_textSpeed);
        }

    }

    void NextSentence()
    {
        if(index < _sentences.Length - 1)
        {
            index++;
            _text.text = string.Empty;
            StartCoroutine(TypeSentence());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


}
