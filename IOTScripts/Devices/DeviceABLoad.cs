using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceABLoad : MonoBehaviour
{
    public string ABSourceFullName = "alldevices.devices";
    public string SourceName = "alldevices";

    private void Start()
    {
        AddObjToScene(ABSourceFullName, SourceName);
    }
    public void AddObjToScene(string abSourceFullName, string sourceName)
    {
        //首先加载包，加载我们创建的abtest包，而Application.streamingAssetsPath为我们拷贝的路径的接口写法
        AssetBundle abFile = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abSourceFullName);

        var file = abFile.LoadAsset(sourceName);

        GameObject obj = Instantiate(file) as GameObject;
        obj.transform.position = Vector3.zero;
        obj.transform.parent = transform;
        //卸载所有加载的AB包，如果参数为True，则同时将AB包加载的资源一并卸载
        AssetBundle.UnloadAllAssetBundles(false);
    }

}
