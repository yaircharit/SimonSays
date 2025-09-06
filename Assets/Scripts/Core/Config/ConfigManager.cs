using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Configs
{
    public static class ConfigManager<T> where T : BaseAppConfig
    {
        private static List<T> _configs;
        private static bool _isLoading;

        public static string FileName => Core.Settings.SettingsManager.Settings.remoteConfigPath;

        public static List<T> Configs
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

        public static async Task<List<T>> LoadConfigsAsync(string path = null)
        {
            if (_isLoading) return null;

            _isLoading = true;
            path ??= (Path.GetExtension(FileName) == ".firebase") ?
                FileName : Path.Combine(Application.streamingAssetsPath, FileName);

            try
            {
                _configs = await ConfigLoader<T>.LoadConfigAsync(path);
                _configs = _configs.OrderBy(c => c.Index).ToList();
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
            return _configs;
        }
    }
}
