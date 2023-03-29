using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityWebSocket;
using LitJson;
using System.IO;
using System.Net;

public class NetWebSocket : UnitySingleton<NetWebSocket>
{
    //private string address = "ws://192.168.255.10:14000/smart/factory/api/websocket/192.168.255.22";
    // 172.16.150.32:14700
    //public string address = "wss://echo.websocket.events";
    private string _ip = "";
   // public string sendText = "Hello UnityWebSocket!";

    private IWebSocket socket;
   // WebSocketState state;
    public bool IsConnect = false;
    private string log = "";
    WebSocketState state;


    public override void Awake()
    {
        base.Awake();
        SetIP();
      
        state = socket == null ? WebSocketState.Closed : socket.ReadyState;
        
        ConnectWebSocket();
    }
  
    private void SetIP() {
        BaseFileOpen.CreateFolder("URL", "ws://172.16.150.32:14701/smart/factory/api/websocket");
    }
    private string GetIP()
    {
        _ip=  BaseFileOpen.ReadFile("URL");
        return _ip;
    }

    private void ConnectWebSocket()
    {
        _ip = GetIP();
      
            socket = new WebSocket(_ip);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            AddLog(string.Format("连接中..."));
            Debug.Log("连接中:"+ _ip);
            socket.ConnectAsync();

        
    }

    public void AddLog(string str)
    {
       
        if (str.Length > 100) str = str.Substring(0, 100) + "...";
        log += System.DateTime.Now + ":    "+ str + "\n";
        if (log.Length > 22 * 1024)
        {
            log = log.Substring(log.Length - 22 * 1024);
        }

        string prjPath = System.Environment.CurrentDirectory + "/Logs";
        //string directoryPath = @"D:\test";//定义一个路径变量
        string filePath = "DebugLog.txt";//定义一个文件路径变量


        if (!Directory.Exists(prjPath))
        {
            Debug.Log("Directory.CreateDirectory=" + prjPath);
            Directory.CreateDirectory(prjPath);
        }
        StreamWriter sw = new StreamWriter(prjPath + "/" + filePath, false, System.Text.Encoding.UTF8);
        sw.Write(log);
        sw.Flush();
        sw.Close();
    }

    private void Socket_OnOpen(object sender, OpenEventArgs e)
    {
        AddLog(string.Format("连接成功: {0}", _ip));
        Debug.Log("连接成功: "+ _ip);
        IsConnect = true;
       // socket.SendAsync("{\"data\":{\"loginStatus\":\"ON_LINE\",\"operCode\":\"ST00002\",\"operName\":\"冲压车间\",\"operOrder\":1,\"userId\":57,\"userName\":\"CY004\"},\"type\":\"user\"}");
    }

    private void Socket_OnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            AddLog(string.Format("Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
        }
        else if (e.IsText)
        {
            AddLog(string.Format("接收: {0}", e.Data));
            Debug.Log(string.Format("接收: {0}", e.Data));
            JsonData data= JsonMapper.ToObject(e.Data);
            if (data["type"].ToString()=="user")//用户消息
            {
                 EventDispatcher.DispatchEvent<string>("user", e.Data);
            }
            else if (data["type"].ToString() == "workOrderSend")//车间消息
            {
                EventDispatcher.DispatchEvent<string>("workOrderSend", e.Data);

            }
            else if (data["type"].ToString() == "login")//登录消息
            {
                EventDispatcher.DispatchEvent<string>("login", e.Data);
            }
        }
       
    }

    private void Socket_OnClose(object sender, CloseEventArgs e)
    {
        AddLog(string.Format("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
        Debug.Log(string.Format("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
        //IsConnect = false;
        //socket = new WebSocket(_ip);
        //socket.OnOpen += Socket_OnOpen;
        //socket.OnMessage += Socket_OnMessage;
        //socket.OnClose += Socket_OnClose;
        //socket.OnError += Socket_OnError;
        //socket.ConnectAsync();
        //ConnectWebSocket();
    }

    private void Socket_OnError(object sender, UnityWebSocket.ErrorEventArgs e)
    {
        AddLog(string.Format("Error: {0}", e.Message));
        Debug.Log(string.Format("Error: {0}", e.Message));
        IsConnect = false;
        ConnectWebSocket();
    }

    private void OnApplicationQuit()
    {
        MessageDate sendDate = new MessageDate();
        Message send = new Message();
        send.message = GCGameManager.Instance.SelfUserId;
        sendDate.data = send;
        sendDate.type = "quit";
        if (send.message!="")
        {
        Send(sendDate);

        }
        //socket.SendAsync(JSONUtil.ToJson(sendDate));
        //AddLog(string.Format("发送: {0}", JSONUtil.ToJson(sendDate)));
        //Debug.Log(System.DateTime.Now + ":" + string.Format("发送: {0}", JSONUtil.ToJson(sendDate)));
        if (socket != null && socket.ReadyState != WebSocketState.Closed)
        {
            socket.CloseAsync();
            Debug.Log("断开");
            AddLog(System.DateTime.Now + ":断开" );
        }

    }

    public void Send(object  date) {
        if (IsConnect)
        {
            socket.SendAsync(JSONUtil.ToJson(date));
            AddLog(string.Format("发送: {0}", JSONUtil.ToJson(date)));
            Debug.Log( System.DateTime.Now+":"+  string.Format("发送: {0}", JSONUtil.ToJson(date)));
        }
        else {
            Debug.Log("未连接服务器");
            AddLog(string.Format("未连接服务器"));
        }
    }

    /// <summary>
    /// 动画结束
    /// </summary>
    /// <param name="count"></param>
    /// <param name="PlayCount"></param>
    public void EndSend(int count ,int PlayCount) {
        if (count == GCGameManager.Instance.AnimationCount)
        {
            MessageDate sendDate = new MessageDate();
            Message send = new Message();
            send.message = "END";
            sendDate.data = send;
            sendDate.type = "workOrder";
            Send(sendDate);
            PlayCount = 0;
        }

    }
   
}

