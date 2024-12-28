using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts;

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
        // Initialize volume slider
        volumeSlider.value = GlobalVariables.Volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Initialize mute button
        muteToggle.isOn = GlobalVariables.Mute;
        muteToggle.onValueChanged.AddListener(ToggleMute);

        // Initialize resolution dropdown
        resolutions = Screen.resolutions; //TODO: only common resolutions
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach ( var res in resolutions )
        {
            options.Add(res.width + " x " + res.height);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GlobalVariables.Resolution;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Initialize fullscreen toggle
        fullscreenToggle.isOn = GlobalVariables.Fullscreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Initialize apply and cancel buttons
        applyButton.onClick.AddListener(ApplySettings);
        cancelButton.onClick.AddListener(CancelSettings);

        // Save initial settings
        initialVolume = GlobalVariables.Volume;
        initialMuteState = GlobalVariables.Mute;
        initialResolutionIndex = GlobalVariables.Resolution;
        initialFullscreenState = GlobalVariables.Fullscreen;
    }

    private void SetVolume(float volume)
    {
        GlobalVariables.Volume = volume;
        AudioListener.volume = volume / 100;
    }

    private void ToggleMute(bool state)
    {
        isMuted = state;
        GlobalVariables.Mute = state;
        AudioListener.volume = isMuted ? 0 : volumeSlider.value / 100;
        muteToggle.GetComponentInChildren<TMP_Text>().text = isMuted ? "Unmute" : "Mute";
        volumeSlider.interactable = !isMuted;
    }

    private void SetResolution(int resolutionIndex)
    {
        GlobalVariables.Resolution = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetFullscreen(bool isFullscreen)
    {
        GlobalVariables.Fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    private void ApplySettings()
    {
        initialVolume = GlobalVariables.Volume;
        initialMuteState = GlobalVariables.Mute;
        initialResolutionIndex = GlobalVariables.Resolution;
        initialFullscreenState = GlobalVariables.Fullscreen;
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    private void CancelSettings()
    {
        GlobalVariables.Volume = initialVolume;
        GlobalVariables.Mute = initialMuteState;
        GlobalVariables.Resolution = initialResolutionIndex;
        GlobalVariables.Fullscreen = initialFullscreenState;

        AudioListener.volume = initialVolume;
        isMuted = initialMuteState;
        volumeSlider.value = initialVolume;
        muteToggle.isOn = initialMuteState;
        resolutionDropdown.value = initialResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        Screen.SetResolution(resolutions[initialResolutionIndex].width, resolutions[initialResolutionIndex].height, initialFullscreenState);
        fullscreenToggle.isOn = initialFullscreenState;

        SceneManager.LoadScene("MainMenu");
    }
}