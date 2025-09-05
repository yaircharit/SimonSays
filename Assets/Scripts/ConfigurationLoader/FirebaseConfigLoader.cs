using System;
using System.Collections.Generic;
using System.IO;
using Firebase;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;
using System.Threading.Tasks;

namespace ConfigurationLoader
{
    /// <summary>
    /// Loads JSON configuration stored as a Remote Config parameter in Firebase.
    /// The class inherits JsonConfigLoader so it can reuse the Deserialize implementation.
    /// </summary>
    public class FirebaseConfigLoader<T> : JsonConfigLoader<T> where T : class
    {
        public FirebaseConfigLoader(string remoteKey) : base(remoteKey)
        {
        }

        // This method should be called from a coroutine or async context
        protected override async Task<string> GetRawDataAsync()
        {
            var depStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (depStatus != DependencyStatus.Available)
                throw new InvalidOperationException("Could not resolve all Firebase dependencies: " + depStatus);

            var rc = FirebaseRemoteConfig.DefaultInstance;
            await rc.FetchAsync(TimeSpan.FromSeconds(30));
            await rc.ActivateAsync();

            var cfgValue = rc.GetValue(configPath).StringValue;
            if (string.IsNullOrEmpty(cfgValue))
                throw new FileNotFoundException($"Remote Config parameter '{configPath}' not found or empty.");

            return cfgValue;
        }
    }
}
