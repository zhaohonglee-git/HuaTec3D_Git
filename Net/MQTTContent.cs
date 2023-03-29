using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using UnityEngine.XR;

public delegate void DOASEventHandel(string msg);

public class MQTTContent : UnitySingleton<MQTTContent>
{

    private MqttClient client;

    //private string ip= "172.16.150.14";
    //private int Port=1883;
    //private string clientId= "gwu1DfBasYk.CompressedAirStationEnv_01|securemode=2,signmethod=hmacsha256,timestamp=1678408950150|";
    //private string username= "huatec_dotnet";
    //private string password= "2C83177e_76f8e4a8";


    private string ip = "iot-06z00isnjub2omb.mqtt.iothub.aliyuncs.com";
    private int Port = 1883;
    private string clientId = "gwu1sCsRwNy.EnviromentMonitor_CAS_01|securemode=2,signmethod=hmacsha256,timestamp=1678868398568|";
    private string username = "EnviromentMonitor_CAS_01&gwu1sCsRwNy";
    private string password = "ca7de5106724e012437c4fbb66451b02270160ccc7b867501871fe4d2f327eca";
    public event DOASEventHandel _DOAS;
    private bool IsCennect;


    public override void Awake()
    {
        base.Awake();
        MqttConnect();
       
    }

    public void Start()
    {
        Loom.Initialize();
    }
    public void MqttConnect() {
        try
        {
            client = new MqttClient(ip, Port, false, null);
            //client = new MqttClient(IPAddress.Parse(ip), Port, false, null);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed; 
            client.Connect(clientId, username, password);
            IsCennect = true;
            Debug.Log("连接成功");

        }
        catch (Exception e)
        {
            Debug.LogError("连接失败：" + e.Message);
            throw;
        }
    }

  

    private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
        Debug.Log("订阅" + e.MessageId);
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        
        string message = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("接收到消息是:" + message);
        JsonData jsonData= JsonMapper.ToObject(message);
        if (jsonData["version"].ToString()=="1.0.0")
        {
            //On_Doas(message);
            Loom.RunAsync(() => {
                Loom.QueueOnMainThread(() => {
                    EventDispatcher.DispatchEvent("DOAS", message);
                });
            });

        }
    }

    //private void MQTTContent__DOAS(string msg)
    //{
    //    Debug.Log("msg"+msg);
    //}



    /// <summary>
    /// 订阅
    /// </summary>
    public void Subscribe(string subscribe_chanel) {

        if (client != null && subscribe_chanel != ""&& IsCennect)
        {
            client.Subscribe(new string[] { subscribe_chanel }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }
    }
    /// <summary>
    /// 发布
    /// </summary>
    public void Publish(string publish_chanel,string publish_content)
    {
        if (client != null && publish_chanel != ""&& IsCennect)
        {
            Debug.Log("发布信息："+publish_content);
            client.Publish(publish_chanel, System.Text.Encoding.UTF8.GetBytes(publish_content), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}

