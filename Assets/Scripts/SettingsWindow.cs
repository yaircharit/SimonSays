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

    public string[] supportedResolutions;
    private bool isMuted = false;
    public Resolution[] resolutions = {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1024, height = 576 },
        new Resolution { width = 800, height = 600 },
        new Resolution { width = 640, height = 480 }
    };
    private float initialVolume;
    private bool initialMuteState;
    private int initialResolutionIndex;
    private bool initialFullscreenState;

    void Awake()
    {
        // Initialize resolution dropdown
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach ( var res in resolutions )
        {
            options.Add(res.width + " x " + res.height);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        // Initialize based on previous settings
        volumeSlider.value = GlobalVariables.Volume;
        muteToggle.isOn = GlobalVariables.Mute;
        fullscreenToggle.isOn = GlobalVariables.Fullscreen;
        resolutionDropdown.value = GlobalVariables.Resolution;

        // Save initial settings to revert back if needed
        initialVolume = GlobalVariables.Volume;
        initialMuteState = GlobalVariables.Mute;
        initialResolutionIndex = GlobalVariables.Resolution;
        initialFullscreenState = GlobalVariables.Fullscreen;
    }

    public void SetVolume()
    {
        GlobalVariables.Volume = volumeSlider.value;
        AudioListener.volume = volumeSlider.value / 100;
    }

    public void ToggleMute()
    {
        isMuted = muteToggle.isOn;
        GlobalVariables.Mute = muteToggle.isOn;
        AudioListener.volume = isMuted ? 0 : volumeSlider.value / 100;
        muteToggle.GetComponentInChildren<TMP_Text>().text = isMuted ? "Unmute" : "Mute";
        volumeSlider.interactable = !isMuted;
    }

    public void SetResolution()
    {
        GlobalVariables.Resolution = resolutionDropdown.value;
        Resolution resolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ToggleFullscreen()
    {
        GlobalVariables.Fullscreen = fullscreenToggle.isOn;
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void ApplySettings()
    {
        initialVolume = GlobalVariables.Volume;
        initialMuteState = GlobalVariables.Mute;
        initialResolutionIndex = GlobalVariables.Resolution;
        initialFullscreenState = GlobalVariables.Fullscreen;
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelSettings()
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