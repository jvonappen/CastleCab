using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    //play audio if source is not already playing this sound
    public void Play(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if (sound == null)
        {
            Debug.Log("Sound: " + name + " not found");
            return;
        }
        if (sound.source.isPlaying) return;
        else sound.source.Play();        
    }

    //stop audio
    public void Stop(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if (sound == null)
        {
            Debug.Log("Sound: " + name + " not found");
            return;
        }
        sound.source.Stop();
    }
}
