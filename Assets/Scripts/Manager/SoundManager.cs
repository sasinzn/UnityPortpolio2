using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundKey
{
    LOBBY_BGM,
    PLAY_BGM,
    FIRE,
    EXPLOSION
}

[Serializable]
public struct SoundData
{
    public SoundKey key;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<SoundData> data;

    private AudioSource audioSource;
    private Dictionary<SoundKey, AudioClip> sounds = new Dictionary<SoundKey, AudioClip>();

    private void Awake()
    {
        instance = this;

        foreach(SoundData sound in data)
        {
            sounds.Add(sound.key, sound.clip);
        }

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(SoundKey key)
    {
        audioSource.clip = sounds[key];
        audioSource.Play();
    }

    public void PlayFX(SoundKey key)
    {
        audioSource.PlayOneShot(sounds[key]);
    }
}
