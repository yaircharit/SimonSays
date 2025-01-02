using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Allows the user to change the volume settings (volume level or mute), to change screen resolution and to enable/disable fullscreen
/// </summary>
public class SettingsWindow : MonoBehaviour
{
    #region PlayerPrefsAccess
    // PlayerPrefs keys
    private const string VolumeKey = "Volume";
    private const string MuteKey = "Mute";
    private const string ResolutionXKey = "ResolutionX";
    private const string ResolutionYKey = "ResolutionY";
    private const string FullscreenKey = "Fullscreen";

    public static float Volume
    {
        get => PlayerPrefs.GetFloat(VolumeKey, 0.5f); // Default volume is 1.0 (100%)
        set { PlayerPrefs.SetFloat(VolumeKey, value); }
    }

    public static bool Mute
    {
        get => PlayerPrefs.GetInt(MuteKey, 0) == 1; // Default is not muted
        set { PlayerPrefs.SetInt(MuteKey, value ? 1 : 0); }
    }

    public static Vector2Int defaultResolution = new Vector2Int(1280, 720);
    public static Resolution DefaultResolution => new Resolution() { width = defaultResolution.x, height = defaultResolution.y };
    public static Resolution Resolution
    {
        get => new() { width = PlayerPrefs.GetInt(ResolutionXKey, DefaultResolution.width), height = PlayerPrefs.GetInt(ResolutionYKey, DefaultResolution.height) };
        set {
            PlayerPrefs.SetInt(ResolutionXKey, value.width);
            PlayerPrefs.SetInt(ResolutionYKey, value.height);
        }
    }

    public static bool Fullscreen
    {
        get => PlayerPrefs.GetInt(FullscreenKey, 1) == 1; // Default is fullscreen
        set { PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0); }
    }
    #endregion PlayerPrefsAccess

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
        availableResolutions ??= Screen.resolutions.Where((reso) => reso.width >= minimalResolution.width && reso.height >= minimalResolution.height).ToArray(); // Load only once (??=)
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach ( var res in availableResolutions )
        {
            options.Add(res.width + " x " + res.height);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        // Initialize based on previous settings
        volumeSlider.value = Volume;
        muteToggle.isOn = Mute;
        fullscreenToggle.isOn = Fullscreen;

        // Update resolutionDropdown value after adding options
        resolutionDropdown.value = System.Array.IndexOf(availableResolutions, availableResolutions.FirstOrDefault(r => r.width == Resolution.width && r.height == Resolution.height));
        resolutionDropdown.RefreshShownValue();

        // Save initial settings to revert back if needed
        initialVolume = Volume;
        initialMuteState = Mute;
        initialResolution = Resolution;
        initialFullscreenState = Fullscreen;
    }

    public void SetVolume()
    {
        Volume = AudioListener.volume =  volumeSlider.value / volumeSlider.maxValue;
    }

    public void ToggleMute()
    {
        Mute = muteToggle.isOn;
        AudioListener.volume = Mute ? 0 : Volume;
        muteToggle.GetComponentInChildren<TMP_Text>().text = Mute ? "Unmute" : "Mute";
        volumeSlider.interactable = !Mute;
    }

    public void SetResolution()
    {
        Resolution = availableResolutions[resolutionDropdown.value];
        Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreen);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = Fullscreen = fullscreenToggle.isOn;
    }

    /// <summary>
    /// Saves settings and moves back to main menu
    /// </summary>
    public void ApplySettings()
    {
        PlayerPrefs.Save(); //Saves changes to PlayerPrefs (actually changed via GlobalVariables on-value-change)
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Reverts all changes made and exits to main menu
    /// </summary>
    public void CancelSettings()
    {
        // Save initial state to GlobalVariables
        Volume = initialVolume;
        Mute = initialMuteState;
        Resolution = initialResolution;
        Fullscreen = initialFullscreenState;

        // Revert changes made
        AudioListener.volume = initialVolume;
        Mute = initialMuteState;
        volumeSlider.value = initialVolume;
        muteToggle.isOn = initialMuteState;
        resolutionDropdown.value = System.Array.IndexOf(availableResolutions, availableResolutions
            .FirstOrDefault(r => r.width == initialResolution.width && r.height == initialResolution.height));
        resolutionDropdown.RefreshShownValue();
        Screen.SetResolution(initialResolution.width, initialResolution.height, initialFullscreenState);
        fullscreenToggle.isOn = initialFullscreenState;

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Loads saved settings
    /// </summary>
    public static void LoadSettings()
    {
        AudioListener.volume = Mute ? 0 : Volume;
        Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreen);
        Screen.fullScreen = Fullscreen;
    }
}
