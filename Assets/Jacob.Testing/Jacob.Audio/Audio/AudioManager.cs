using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioDetails[] musicAudio, sfxAudio;
    public AudioSource musicSource, sfxSource, achvSource, vatSource, marketSource; //Add extra sources to play various sfx at once

    public AudioGroupDetails[] audioGroups;

    [SerializeField] private Slider musicSlider, sfxSlider; //masterSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Ye");
        MusicVolume(musicSlider.value);
        SFXVolume(sfxSlider.value);
    }

    public void PlayMusic(string name)
    {
        AudioDetails audio = Array.Find(musicAudio, x => x.audioName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        
        if (musicSource.isPlaying) return;
        else
        {
            musicSource.clip = audio.clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        AudioDetails audio = Array.Find(sfxAudio, x => x.audioName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        if (sfxSource.isPlaying) return;
        else
        {
            sfxSource.clip = audio.clip;
            sfxSource.PlayOneShot(audio.clip);
        }

    }
    public void PlaySFX_Achv(string name)
    {
        AudioDetails audio = Array.Find(sfxAudio, x => x.audioName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        if (achvSource.isPlaying) return;
        else
        {
            achvSource.clip = audio.clip;
            achvSource.PlayOneShot(audio.clip);
        }

    }

    public void PlayGroupAudio(string name)
    {
        AudioGroupDetails audio = Array.Find(audioGroups, x => x.audioGroupName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        if (sfxSource.isPlaying) return;
        else
        {
            int randomVal = UnityEngine.Random.Range(0, audio.audioClips.Length);
            sfxSource.clip = audio.audioClips[randomVal];
            sfxSource.PlayOneShot(audio.audioClips[randomVal]);
        }
    }

    public void PlayGroupAudio_Vat(string name)
    {
        AudioGroupDetails audio = Array.Find(audioGroups, x => x.audioGroupName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        if (vatSource.isPlaying) return;
        else
        {
            int randomVal = UnityEngine.Random.Range(0, audio.audioClips.Length);
            vatSource.clip = audio.audioClips[randomVal];
            vatSource.PlayOneShot(audio.audioClips[randomVal]);
        }
    }
    public void PlayGroupAudio_Market(string name)
    {
        AudioGroupDetails audio = Array.Find(audioGroups, x => x.audioGroupName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        if (marketSource.isPlaying) return;
        else
        {
            int randomVal = UnityEngine.Random.Range(0, audio.audioClips.Length);
            marketSource.clip = audio.audioClips[randomVal];
            marketSource.PlayOneShot(audio.audioClips[randomVal]);
        }
    }
    public void StopMusic(string name)
    {
        AudioDetails audio = Array.Find(musicAudio, x => x.audioName == name);
        if (audio == null) return;

        if (musicSource.isPlaying) musicSource.Stop();
        else return;
    }

    public void StopSFX()
    {
        AudioDetails audio = Array.Find(sfxAudio, x => x.audioName == name);
        if (audio == null) return;
        if (sfxSource.isPlaying) sfxSource.Stop();
        else return;
    }

    public void ToggleMusic() { musicSource.mute = !musicSource.mute; }
    public void ToggleSFX() { sfxSource.mute = !sfxSource.mute; }
    public void MusicVolume(float volume) { volume = musicSlider.value; musicSource.volume = volume; }
    public void SFXVolume(float volume) { volume = sfxSlider.value; sfxSource.volume = volume; }

}
