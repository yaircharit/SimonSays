using UnityEngine;

namespace Core.Settings
{

    [CreateAssetMenu(fileName = "AppSettings", menuName = "AppSettings")]
    public class AppSettings : ScriptableObject
    {
        [SerializeField]
        public string configPath = "config.firebase";
        [SerializeField]
        public string databaseFilePath = "SimonSays.firebase";
        [SerializeField]
        public string leaderboardTableName = "Leaderboard";

        [Header("Firebase Settings")]
        [SerializeField]
        public string firebaseRestEndpoint = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
        //[SerializeField]
        //public string loginMethod = "signUp"; // or "email"
        [SerializeField]
        public string databasePath = "https://your-project-id.firebaseio.com/";
        [SerializeField]
        public string apiKey = "your-api";

    }
}