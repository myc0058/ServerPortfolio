using Schema.Protobuf;
using System;
using System.Diagnostics;
using UnityEngine;

public class Network : MonoBehaviour
{
    void Awake()
    {
        Api.StartUp();
    }
    
    void Start()
    {
    }

    void Update()
    {
        var clientList = Client.GetClientList();
        foreach (var item in clientList)
        {
            Callback msgEvent = item.GetMsgCallback();
            while (msgEvent != null)
            {
                msgEvent.Invoke();
                msgEvent = item.GetMsgCallback();
            }
        }

        foreach (var item in clientList)
        {
            if (item.User != null && item.User.Idx != 0)
            {
                item.User.CheckAndSendSyncTime();
            }
        }
    }

    void OnDestroy()
    {
        var clientList = Client.GetClientList();
        foreach (var item in clientList)
        {
            item.Reconnect = false;
            item.Disconnect();
        }
    }

    public string IP;
    public ushort Port;
    public int ClientCount;

    public static Network GetScriptComponent()
    {
        return GameObject.Find("Network").GetComponent<Network>();
    }
}
