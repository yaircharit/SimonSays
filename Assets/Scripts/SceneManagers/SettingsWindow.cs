using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Resolution minimalResolution = new Resolution() { width=600, height=600 };

    private static Resolution[] availableResolutions;
    private float initialVolume;
    private bool initialMuteState;
    private Resolution initialResolution;
    private bool initialFullscreenState;

    void Awake()
    {
        // Initialize resolution dropdown
        availableResolutions ??= Screen.resolutions.Where((reso) => reso.width >= minimalResolution.width && reso.height >= minimalResolution.height).ToArray(); // Load only once
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach ( var res in availableResolutions )
        {
            options.Add(res.width + " x " + res.height);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        // Initialize based on previous settings
        volumeSlider.value = GlobalVariables.Volume;
        muteToggle.isOn = GlobalVariables.Mute;
        fullscreenToggle.isOn = GlobalVariables.Fullscreen;

        // Update resolutionDropdown value after adding options
        resolutionDropdown.value = System.Array.IndexOf(availableResolutions, availableResolutions.FirstOrDefault(r => r.width == GlobalVariables.Resolution.width && r.height == GlobalVariables.Resolution.height));
        resolutionDropdown.RefreshShownValue();

        // Save initial settings to revert back if needed
        initialVolume = GlobalVariables.Volume;
        initialMuteState = GlobalVariables.Mute;
        initialResolution = GlobalVariables.Resolution;
        initialFullscreenState = GlobalVariables.Fullscreen;
    }

    public void SetVolume()
    {
        GlobalVariables.Volume = AudioListener.volume =  volumeSlider.value / volumeSlider.maxValue;
    }

    public void ToggleMute()
    {
        GlobalVariables.Mute = muteToggle.isOn;
        AudioListener.volume = GlobalVariables.Mute ? 0 : GlobalVariables.Volume;
        muteToggle.GetComponentInChildren<TMP_Text>().text = GlobalVariables.Mute ? "Unmute" : "Mute";
        volumeSlider.interactable = !GlobalVariables.Mute;
    }

    public void SetResolution()
    {
        GlobalVariables.Resolution = availableResolutions[resolutionDropdown.value];
        Screen.SetResolution(GlobalVariables.Resolution.width, GlobalVariables.Resolution.height, Screen.fullScreen);
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
        initialResolution = GlobalVariables.Resolution;
        initialFullscreenState = GlobalVariables.Fullscreen;
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelSettings()
    {
        GlobalVariables.Volume = initialVolume;
        GlobalVariables.Mute = initialMuteState;
        GlobalVariables.Resolution = initialResolution;
        GlobalVariables.Fullscreen = initialFullscreenState;

        AudioListener.volume = initialVolume;
        GlobalVariables.Mute = initialMuteState;
        volumeSlider.value = initialVolume;
        muteToggle.isOn = initialMuteState;
        resolutionDropdown.value = System.Array.IndexOf(availableResolutions, availableResolutions
            .FirstOrDefault(r => r.width == initialResolution.width && r.height == initialResolution.height));
        resolutionDropdown.RefreshShownValue();
        Screen.SetResolution(initialResolution.width, initialResolution.height, initialFullscreenState);
        fullscreenToggle.isOn = initialFullscreenState;

        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadSettings()
    {
        AudioListener.volume = GlobalVariables.Mute ? 0 : GlobalVariables.Volume;
        Screen.SetResolution(GlobalVariables.Resolution.width, GlobalVariables.Resolution.height, Screen.fullScreen);
        Screen.fullScreen = GlobalVariables.Fullscreen;
    }
}
