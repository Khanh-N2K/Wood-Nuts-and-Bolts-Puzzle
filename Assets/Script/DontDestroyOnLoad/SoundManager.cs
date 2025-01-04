using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    public float MusicVolume { get => musicSource.volume; }

    [SerializeField] private AudioSource sfxSource;
    public float SFX_Volume { get => sfxSource.volume; }

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private List<SoundClip> sfxClips;

    private Dictionary<SoundEffect, AudioClip> sfxDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InitializeSFXDictionary();
        PlayMusic();
    }

    private void InitializeSFXDictionary()
    {
        sfxDictionary = new Dictionary<SoundEffect, AudioClip>();
        foreach (var soundClip in sfxClips)
        {
            sfxDictionary.Add(soundClip.soundEffect, soundClip.clip);
        }
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(SoundEffect soundEffect)
    {
        if (sfxDictionary.ContainsKey(soundEffect))
        {
            sfxSource.PlayOneShot(sfxDictionary[soundEffect]);
        }
        else
        {
            Debug.LogWarning("SoundManager: SFX not found in dictionary - " + soundEffect);
        }
    }

    public void StopSFX()
    {
        this.sfxSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

[System.Serializable]
public struct SoundClip
{
    public SoundEffect soundEffect;
    public AudioClip clip;
}

public enum SoundEffect
{
    Pin,
    Unpin,
    Button,
    Button2,
    Tap,
    OnWin,
    PopupShow,
    Bomb,
    Pick,
    Hit1,
    Hit2,
    Hit3,
    Place,
    OnLose,
    ClockTicking,
    Stamp
}
