using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace ConfigurationLoader
{
    public class FirebaseRestAPI
    {
        private static string baseUrl = "https://ycgithubio-default-rtdb.europe-west1.firebasedatabase.app/";
        

        public static async Task<string> Get(string path)
        {
            string url = $"{baseUrl}{path}.json";
            Debug.Log(url);
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                var op = www.SendWebRequest();
                while (!op.isDone) await Task.Yield();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }
                return www.downloadHandler.text;
            }
        }

        public static async Task Put(string path, string json)
        {
            string url = $"{baseUrl}{path}.json";
            using (UnityWebRequest request = UnityWebRequest.Put(url, json))
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.SetRequestHeader("Content-Type", "application/json");
                var op = request.SendWebRequest();
                while (!op.isDone) await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Firebase Put error: " + request.error);
                }
                else
                {
                    Debug.Log("Firebase Put success: " + request.downloadHandler.text);
                }
            }
        }
    }
}
