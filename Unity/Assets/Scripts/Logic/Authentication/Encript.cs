using Schema.Protobuf.Message.Authentication;
using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, Encript msg)
        {
            base.OnMessage(notifier, msg);

            if (msg.Key.Length > 0)
            {
                var aes = Aes.Create();
                aes.Key = Convert.FromBase64String(msg.Key);
                aes.IV = Convert.FromBase64String(msg.IV);
                aes.Mode = CipherMode.ECB;

                Client.aesAlg = aes;
            }

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());

            var login = new Login();
            login.ClientPlatform = Schema.Protobuf.Message.Enums.ClientPlatform.Editor;
            login.Id = "momo_" + Client?.UID;
            Client?.Notify(login);

            Debug.Log("Send Login : " + login.Id);
        }
    }
}

