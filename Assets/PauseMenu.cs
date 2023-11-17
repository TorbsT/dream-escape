using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public float Master => settings.MasterVolume;
    public float Music => settings.MusicVolume;
    public float SFX => settings.SFXVolume;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private SettingsObject settings;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    public void Show() => SetActive(true);
    public void Hide() => SetActive(false);
    public void SetActive(bool value, bool doSound = true)
    {
        string soundName = value ? "pause0" : "pause1";
        if (doSound) AudioManager.Instance.Play(soundName);
        pauseMenu.SetActive(value);
    }
    public void SelectLevel(GameObject levelGO)
    {
        Dunkelheit.Instance.FadeTo(levelGO.name);
    }
    public void SetMasterVolume(float volume)
    {
        SetVolume("Master", volume);
        settings.MasterVolume = volume;
    }
    public void SetSFXVolume(float volume)
    { SetVolume("SFX", volume);
        settings.SFXVolume = volume;
    }
    public void SetMusicVolume(float volume)
    { SetVolume("Music", volume);
        settings.MusicVolume = volume;
    }
    private void Awake()
    {
        Instance = this;
        SetActive(false, false);
        masterSlider.value = settings.MasterVolume;
        musicSlider.value = settings.MusicVolume;
        sfxSlider.value = settings.SFXVolume;
        SetVolume("Master", settings.MasterVolume);
        SetVolume("Music", settings.MusicVolume);
        SetVolume("SFX", settings.SFXVolume);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetActive(!pauseMenu.activeSelf);
    }
    private void SetVolume(string key, float value)
    {
        value = LinearToDecibel(value);
        audioMixer.SetFloat(key, value);
    }
    private float LinearToDecibel(float linear)
    {
        // Avoid negative infinity (silence) as a volume by setting a minimum linear value
        float minLinear = 0.0001f;
        return linear > minLinear ? 20f * Mathf.Log10(linear) : -80f;
    }
}
