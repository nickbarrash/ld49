using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Http
{
    public static void get(MonoBehaviour coroutineRunner, string url, Action<long, string> callback, Dictionary<string, string> headers = null) {
        coroutineRunner.StartCoroutine(getCoroutine(url, callback, headers));
    }

    private static IEnumerator getCoroutine(string url, Action<long, string> callback, Dictionary<string, string> headers = null) {
        using (var request = new UnityWebRequest(url, "GET")) {
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //request.SetRequestHeader("Content-Type", "application/json");
            if (headers != null) {
                foreach (var header in headers) {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }
            yield return request.SendWebRequest();
            callback(request.responseCode, request.downloadHandler.text);
        }
    }

    public static void post(MonoBehaviour coroutineRunner, string url, string body, Action<long, string> callback, Dictionary<string, string> headers = null) {
        coroutineRunner.StartCoroutine(postCoroutine(url, body, callback, headers));
    }

    private static IEnumerator postCoroutine(string url, string body, Action<long, string> callback, Dictionary<string, string> headers = null)
    {
        using (var request = new UnityWebRequest(url, "POST")) {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (headers != null) {
                foreach(var header in headers) {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }
            yield return request.SendWebRequest();
            callback(request.responseCode, request.downloadHandler.text);
        }
    }
}
