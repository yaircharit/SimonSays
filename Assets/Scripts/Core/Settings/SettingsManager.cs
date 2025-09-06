using UnityEngine;

namespace Core.Settings
{
    public class SettingsManager
    {
        private static AppSettings _settings;

        public static AppSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<AppSettings>("AppSettings");
                    if (_settings == null)
                    {
                        Debug.LogError("AppSettings.asset not found in Resources!");
                    }
                }
                return _settings;
            }
        }
    }
}

