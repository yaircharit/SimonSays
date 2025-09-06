using UnityEngine;

[CreateAssetMenu(fileName = "AppSettings", menuName = "AppSettings")]
public class AppSettings : ScriptableObject
{
    [Header("Configuration Settings")]
    [SerializeField]
    public string configPath = "config.firebase"; 
    [SerializeField]
    public string fallbackConfigPath = "config.json"; 

    [Header("Database Settings")]
    [SerializeField]
    public string dbFilePath = "SimonSays.firebase";
    [SerializeField]
    public string fallbackDbFilePath = "leaderboard.db";
    [SerializeField]
    public string tableName = "Leaderboard";

    [Header("Firebase Settings")]
    [SerializeField]
    public string firebaseRestEndpoint = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
    [SerializeField]
    public string databaseUrl = "https://xxx.firebasedatabase.app/";
    [SerializeField]
    public string apiKey = "your-api";

}