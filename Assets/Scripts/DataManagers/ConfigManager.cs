using Assets.Scripts;
using ConfigurationLoader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class ConfigManager
{
    private static List<AppConfig> _configs;
    private static bool _isLoading;

    private static ConfigSettings _settings;

    public static ConfigSettings Settings
    {
        get
        {
            if (_settings == null)
            {
                _settings = Resources.Load<ConfigSettings>("ConfigSettings");
                if (_settings == null)
                {
                    Debug.LogError("ConfigSettings.asset not found in Resources!");
                }
            }
            return _settings;
        }
    }

    public static string FileName => Settings.configFileName;

    public static List<AppConfig> Configs
    {
        get
        {
            if (_configs == null && !_isLoading)
            {
                // Lazy load synchronously here, or kick off async/coroutine
                LoadConfigsAsync().GetAwaiter().GetResult();
            }
            return _configs;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnAppStart()
    {
        Debug.Log("Application started — loading configs...");
        LoadConfigsAsync().GetAwaiter();
    }

    public static async Task LoadConfigsAsync()
    {
        if (_configs != null || _isLoading) return;

        _isLoading = true;
        string path = (Path.GetExtension(FileName) == ".firebase") ? 
            FileName : Path.Combine(Application.streamingAssetsPath, FileName);

        try
        {
            _configs = await ConfigLoader<AppConfig>.LoadConfigAsync(path);
            _configs = _configs.OrderBy(c => c.PointsEachStep).ToList();
            Debug.Log($"[ConfigManager] Loaded {_configs.Count} configs");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ConfigManager] Failed to load configs: {ex}");
        }
        finally
        {
            _isLoading = false;
        }
    }
}
