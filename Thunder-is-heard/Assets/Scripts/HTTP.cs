using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HTTP: MonoBehaviour
{

    public void GetRequest(string url)
    {
        StartCoroutine(Get(url));
    }

    public void PostRequest(string url, object requestBody)
    {
        StartCoroutine(Post(url, requestBody));
    }

    private IEnumerator Get(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error + ", " + request.responseCode);
            }
            else {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator Post(string uri, object requestBody)
    {
        var jsonData = JsonUtility.ToJson(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error + ", " + request.responseCode);
            }
            else {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}