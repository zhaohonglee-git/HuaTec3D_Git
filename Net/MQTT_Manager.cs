using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine;
using UnityEngine.UI;

public class MQTT_Manager : MonoBehaviour
{
    [SerializeField] private InputField ip;
    [SerializeField] private InputField port;
    [SerializeField] private InputField subscribe_chanel;
    [SerializeField] private InputField publish_chanel;
    [SerializeField] private InputField publish_content;
    [SerializeField] private Button connect_btn;
    [SerializeField] private Button subscribe_btn;

    [SerializeField] private Button publish_btn;
    [SerializeField] private Text receive_Message_text;
    private MqttClient client;

    private string receive_message;
    // Use this for initialization
    void Start () {
        //连接按钮监听事件
		connect_btn.onClick.AddListener(() =>
		{
            //string txtIP = ip.text;
            //string txtPort = port.text;
            //string clientId = "gwu1ML";
            //      //服务器默认密码是这个
            //string username = "huatec_dotnet";
            //string password = "2C83177e_76f8e4a8";
            //client = new MqttClient(IPAddress.Parse("172.16.150.14"), int.Parse("1883"), false, null);
            string txtIP = ip.text;
            string txtPort = port.text;
            string clientId = "gwu1MLeAnm5.HZ-II-A01-No.22002|securemode=2,signmethod=hmacsha256,timestamp=1678176334162|";
            //服务器默认密码是这个
            string username = "HZ-II-A01-No.22002&gwu1MLeAnm5";
            string password = "c5ed09529700d6f393e6a9c2f779f6ac7f1910fa663d9fc4c4371e9ce4a6912e";
            client = new MqttClient("iot-06z00isnjub2omb.mqtt.iothub.aliyuncs.com", 1883, false, null);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed; ;
            client.Connect(clientId, username, password);
            Debug.Log("连接成功");
            

		});
        //订阅按钮监听事件
        subscribe_btn.onClick.AddListener(() =>
        {
            if (client != null&&subscribe_chanel.text!="")
            {
                client.Subscribe(new string[] { subscribe_chanel.text }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
        });

        //发布按钮监听事件
        publish_btn.onClick.AddListener(() =>
        {
            if (client != null && publish_chanel.text != "")
            {
                client.Publish(publish_chanel.text, System.Text.Encoding.UTF8.GetBytes(publish_content.text), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        });

    }

    private void Client_MqttMsgSubscribed(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs e)
    {
        Debug.Log("订阅" + e.MessageId);
    }

    private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
        string message=System.Text.Encoding.UTF8.GetString(e.Message);
        receive_message = message;
        Debug.Log("接收到消息是"+message);
    }

    // Update is called once per frame
    void Update ()
    {
        if (receive_message != null)
        {
            receive_Message_text.text = receive_message;
        }
       
    }
}
