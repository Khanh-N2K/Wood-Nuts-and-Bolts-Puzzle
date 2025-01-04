using UnityEngine;
using UnityEngine.UI;

public class Sound_UI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Toggle vibrateToggle;

    private void Start()
    {
        this.SetParameters();
        this.AddListener();
    }

    private void SetParameters()
    {
        this.musicSlider.value = SoundManager.Instance.MusicVolume;
        this.sfxSlider.value = SoundManager.Instance.SFX_Volume;
        this.muteToggle.isOn = AudioListener.pause;
        this.vibrateToggle.isOn = Effect.allowVibrate;
    }

    private void AddListener()
    {
        this.musicSlider.onValueChanged.AddListener(SetMusicVolume);
        this.sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        this.muteToggle.onValueChanged.AddListener(MuteVolume);
        this.vibrateToggle.onValueChanged.AddListener(this.SetVibrate);
    }

    public void SetMusicVolume(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }

    public void MuteVolume(bool mute)
    {
        AudioListener.pause = mute;
    }

    public void SetVibrate(bool vibrate)
    {
        Effect.allowVibrate = vibrate;
    }
}
