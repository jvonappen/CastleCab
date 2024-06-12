using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [SerializeField] string m_sound;

    private void Start() => PlaySound();
    void PlaySound()
    {
        TimerManager.RunAfterTime(() => { PlaySound(); }, 0.5f);
        AudioManager.Instance.PlaySoundAtLocation(m_sound, transform.position);
    }
}
