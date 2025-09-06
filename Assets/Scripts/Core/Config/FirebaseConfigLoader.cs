using System;
using System.Threading.Tasks;
using Core.Network.Firebase;

namespace Core.Configs
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
            var task = FirebaseRestAPI.Get(FirebaseRestAPI.GetURL(configPath));
            await task;
            if (task.IsFaulted)
            {
                throw new Exception("Error fetching data from Firebase: " + task.Exception);
            }
            return task.Result;
        }
    }
}
