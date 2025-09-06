using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Network
{
    public class RestAPI
    {
        public static async Task<string> Get(string path)
        {
            return await WebRequest(path, UnityWebRequest.kHttpVerbGET);
        }

        public static async Task<string> Put(string path, string json)
        {
            return await WebRequest(path, UnityWebRequest.kHttpVerbPUT, json);
        }

        public static async Task<string> Post(string path, string json)
        {
            return await WebRequest(path, UnityWebRequest.kHttpVerbPOST, json);
        }

        public static async Task<string> Delete(string path)
        {
            return await WebRequest(path, UnityWebRequest.kHttpVerbDELETE);
        }

        private static async Task<string> WebRequest(string path, string method, string json = null)
        {
            //await Login(); // Ensure logged in
            using (UnityWebRequest request = new UnityWebRequest(path, method))
            {
                if (json != null)
                {
                    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.SetRequestHeader("Content-Type", "application/json");
                }
                request.downloadHandler = new DownloadHandlerBuffer();
                return await HandleRequest(request);
            }
        }

        private static async Task<string> HandleRequest(UnityWebRequest request)
        {
            Debug.Log($"[RestAPI]: {request.method} {request.url}");

            var op = request.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request error: " + request.error);
                Debug.LogError("Error body: " + request.downloadHandler.text);
                return null;
            }
            else
            {
                Debug.Log("Request success: " + request.downloadHandler.text);
                return request.downloadHandler.text;
            }
        }
    }
}
