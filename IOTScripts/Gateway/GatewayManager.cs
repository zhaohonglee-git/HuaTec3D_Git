using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class GatewayManager : MonoBehaviour
{
    [Header("MQTT连接参数")]
    public string ClientId;
    public string UserName;
    public string MqttHostUrl;
    public string Passwd;
    public string Port;

    [Header("连接")]
    public string GatewayName;
    public bool Connect;
    [SerializeField] private bool _isConnected;
    public bool IsConnected { get => _isConnected; private set => _isConnected = value; }

    [Header("发布")]
    public string PubTopic;
    public string Payload;
    [Header("订阅")]
    public string SubTopoic;
    public string ReceivedResult;

    

}
