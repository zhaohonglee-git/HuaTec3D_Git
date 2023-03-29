using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class HttpContent 
{
    /// <summary>
    /// post请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static IEnumerator PostRequest(string url,string data)
    {
        
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            //设置header
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("authKey", "leohui");

            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static IEnumerator GetRequest(string url)
    {
       
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {

            //设置header
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("authKey", "leohui");

            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
