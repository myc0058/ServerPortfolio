using Schema.Protobuf;
using Schema.Protobuf.Message.Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    void OnConnect(Tcp sender, bool ret)
    {
        Client client = sender as Client;
        var ip = client?.IP;
        var port = client?.Port;

        if (ret == true)
        {
            Debug.Log($"Connect Success address : {ip}:{port}");
        }
        else
        {
            Debug.Log($"Connect Fail address : {ip}:{port}");
        }
    }


    void OnDisconnect(Tcp sender)
    {
        Client client = sender as Client;
        User user = client?.User;

        Debug.Log($"OnDisconnect Idx : {user?.Idx}, Id : {user?.ID}");
    }

    public void StartClick()
    {
        var network = Network.GetScriptComponent();

        for (int i = 0; i < network.ClientCount; i++)
        {
            Client client = new Client();
            client.UID = i;
            client.OnConnect += OnConnect;
            client.OnDisconnect += OnDisconnect;

            Debug.Log($"MakeClient : {i}");

            client.Connect(network.IP, network.Port);
        }
    }

    public void EnterRoomClick()
    {
        var msg = new Schema.Protobuf.Message.Game.EnterRoom();
        var clientList = Client.GetClientList();
        foreach (var item in clientList)
        {
            item.Notify(msg);
        }
    }

    public void ChatClick()
    {
        var clientList = Client.GetClientList();
        foreach (var item in clientList)
        {
            if (item.User == null)
            {
                continue;
            }

            var msg = new Schema.Protobuf.Message.Game.Chat();
            msg.Idx = item.User.Idx;
            msg.Id = item.User.ID;
            msg.Msg = "chatting message!!!~~~~";
            item.Notify(msg);
        }
    }

    public void LogoutClick()
    {
        var msg = new Schema.Protobuf.Message.Lobby.Logout();
        var clientList = Client.GetClientList();
        foreach (var item in clientList)
        {
            item.Notify(msg);
        }
    }
}
