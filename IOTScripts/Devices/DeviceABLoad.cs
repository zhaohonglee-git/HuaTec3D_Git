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
        //���ȼ��ذ����������Ǵ�����abtest������Application.streamingAssetsPathΪ���ǿ�����·���Ľӿ�д��
        AssetBundle abFile = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abSourceFullName);

        var file = abFile.LoadAsset(sourceName);

        GameObject obj = Instantiate(file) as GameObject;
        obj.transform.position = Vector3.zero;
        obj.transform.parent = transform;
        //ж�����м��ص�AB�����������ΪTrue����ͬʱ��AB�����ص���Դһ��ж��
        AssetBundle.UnloadAllAssetBundles(false);
    }

}
