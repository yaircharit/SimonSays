using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsWindow : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Button applyButton;
    public Button cancelButton;

    private bool isMuted = false;
    private Resolution[] resolutions;
    private float initialVolume;
    private bool initialMuteState;
    private int initialResolutionIndex;
    private bool initialFullscreenState;

    void Awake()
    {
        gameObject.SetActive(false);

        // Initialize volume slider
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Initialize mute button
        muteToggle.onValueChanged.AddListener((state) => ToggleMute(state));

        // Initialize resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach ( var res in resolutions )
        {
            options.Add(res.width + " x " + res.height);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Initialize fullscreen toggle
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Initialize apply and cancel buttons
        applyButton.onClick.AddListener(ApplySettings);
        cancelButton.onClick.AddListener(CancelSettings);

        // Save initial settings
        initialVolume = AudioListener.volume;
        initialMuteState = isMuted;
        initialResolutionIndex = resolutionDropdown.value;
        initialFullscreenState = Screen.fullScreen;
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void ToggleMute(bool state)
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : volumeSlider.value / 100;
        muteToggle.GetComponentInChildren<Text>().text = isMuted ? "Unmute" : "Mute";
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private int GetCurrentResolutionIndex()
    {
        for ( int i = 0; i < resolutions.Length; i++ )
        {
            if ( resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height )
            {
                return i;
            }
        }
        return 0;
    }

    private void ApplySettings()
    {
        initialVolume = AudioListener.volume;
        initialMuteState = isMuted;
        initialResolutionIndex = resolutionDropdown.value;
        initialFullscreenState = Screen.fullScreen;

        gameObject.SetActive(false);
    }

    private void CancelSettings()
    {
        AudioListener.volume = initialVolume;
        isMuted = initialMuteState;
        volumeSlider.value = initialVolume;
        muteToggle.isOn = !initialMuteState;
        resolutionDropdown.value = initialResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        Screen.SetResolution(resolutions[initialResolutionIndex].width, resolutions[initialResolutionIndex].height, initialFullscreenState);
        fullscreenToggle.isOn = initialFullscreenState;

        gameObject.SetActive(false);
    }
}
