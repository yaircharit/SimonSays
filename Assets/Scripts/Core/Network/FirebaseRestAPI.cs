using Core.Settings;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Network.Firebase
{
    [System.Serializable]
    public class FirebaseLoginResponse
    {
        public string idToken;      // JWT, used for database requests
        public string refreshToken; // To renew idToken when expired
        public string localId;      // Firebase UID
        public string email;        // User’s email
    }

    public class FirebaseRestAPI : RestAPI
    {
        private static string baseUrl = SettingsManager.Settings.databaseUrl;
        private static string apiKey = SettingsManager.Settings.apiKey;
        private static string restEndpoint = SettingsManager.Settings.firebaseRestEndpoint;

        private static readonly object loginBody = new { returnSecureToken = true };
        private static string loginBodyJson => JsonUtility.ToJson(loginBody);
        private static FirebaseLoginResponse loginResponse;
        private static bool isLoggedIn => loginResponse != null;
        public static string GetURL(string path) => $"{baseUrl}{path}.json{(isLoggedIn ? $"?auth={loginResponse.idToken}" : "")}";
        public static Task<FirebaseLoginResponse> loginTask;

        public static async Task<FirebaseLoginResponse> Login(string databaseUrl = null, string webApiKey = null)
        {

            if (loginResponse != null)
            {
                Debug.Log("Already logged in to Firebase.");
                return loginResponse; // Already logged in
            }

            baseUrl = databaseUrl ?? SettingsManager.Settings.databaseUrl;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            apiKey = webApiKey ?? SettingsManager.Settings.apiKey;
            if (apiKey.StartsWith("/")) apiKey = apiKey.Substring(1);

            // Login with api key
            var result = Post(restEndpoint + apiKey, loginBodyJson);
            await result;

            if (result.IsFaulted)
            {
                Debug.LogError("[Firebase] Login error: " + result.Exception);
                return null;
            }

            Debug.Log("[Firebase] Login successful.");
            return loginResponse = JsonUtility.FromJson<FirebaseLoginResponse>(result.Result);
        }
    }
}
