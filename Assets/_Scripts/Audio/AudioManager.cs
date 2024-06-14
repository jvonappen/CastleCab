using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioDetails[] musicAudio, sfxAudio;
    public AudioSource musicSource, sfxSource;

    public AudioGroupDetails[] audioGroups;

    [SerializeField] private Slider musicSlider, sfxSlider; //masterSlider;

    [SerializeField] float m_soundRange = 30;

    List<Transform> players = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic("Exalted");
        //PlayMusic("TROUBADOUR");
        PlayMusic("KnightRidersBlueBird");
        //PlayMusic("Ye");
        
        MusicVolume(musicSlider.value);
        SFXVolume(sfxSlider.value);

        GameManager.Instance.onPlayerAdd += OnPlayerAdd;
        OnPlayerAdd();
    }

    public void OnPlayerAdd()
    {
        players = GameManager.Instance.players.Select(x => x.player.transform.GetChild(0)).ToList(); // Gets horse transforms, not base player
    }

    public void PlayMusic(string name)
    {
        AudioDetails audio = Array.Find(musicAudio, x => x.audioName == name);
        if (audio == null) { Debug.Log("Audio not found"); }

        //if (musicSource.isPlaying) return;
        else
        {
            musicSource.clip = audio.clip;
            musicSource.Play();

        }

    }

    public void PlaySoundAtDistance(string _soundName, float _distance)
    {
        // Play group audio
        AudioGroupDetails audio = Array.Find(audioGroups, x => x.audioGroupName == _soundName);
        if (audio == null) { Debug.Log("Audio not found"); }
        else
        {
            int randomVal = UnityEngine.Random.Range(0, audio.audioClips.Length);
            sfxSource.clip = audio.audioClips[randomVal];

            float volume = Mathf.Clamp(1 - (_distance/ m_soundRange), 0, 1); // Sets volume based on distance and max range

            sfxSource.PlayOneShot(audio.audioClips[randomVal], volume);
        }
    }

    public void PlaySoundAtLocation(string _soundName, Vector3 _worldPos)
    {
        // Gets closest player distance and plays sound loudness accordingly
        Transform closestPlayer = players.OrderBy(player => (player.position - _worldPos).sqrMagnitude).FirstOrDefault();
        if (closestPlayer) PlaySoundAtDistance(_soundName, Vector3.Distance(_worldPos, closestPlayer.position));
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



    #region Deprecated

    /// <summary>
    /// deprecated. Use 'PlaySoundAtDistance' (Requires AudioGroup sound)
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        AudioDetails audio = Array.Find(sfxAudio, x => x.audioName == name);
        if (audio == null) { Debug.Log("Audio not found"); }
        //if (sfxSource.isPlaying) return;
        else
        {
            sfxSource.clip = audio.clip;
            sfxSource.PlayOneShot(audio.clip);
        }
    }

    #endregion
}
