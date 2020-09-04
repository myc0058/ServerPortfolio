using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Engine.Framework.Api;
using Engine.Network.Protocol;

namespace Application.Agent.Delegatables
{
    public class Client : Engine.Network.Protocol.Delegator<Client>.IDelegatable
    {
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        { }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Client>.Notifier notifier, int code, MemoryStream stream)
        {
            var client = Singleton<Entities.Client>.Instance;

            Engine.Framework.AsyncCallback callback = null;


            var type = Schema.Protobuf.Api.CodeToType(code);

            var method = client.GetType().GetMethod("OnMessage", new Type[] {
                typeof(Engine.Network.Protocol.Delegator<Client>.Notifier),
                type
            });


            //if (code == Engine.Framework.Id<Schema.Protobuf.Message.Administrator.Publish>.Value)
            //{
            //    callback = Engine.Network.Api.Binder(client, notifier, code, stream);
            //}

            if (method != null)
            {
                callback = Engine.Network.Api.Binder(client, notifier, code, stream);
                client.PostMessage(callback);

                return;
            }

            // 일반적으론 에이전트클라이언트에 연결된 프로세스가 하나여야하지만
            // 스탠드얼론 같은 경우 여러개일수있다.
            // 또는 서버가 통합되어 하나의 머신에 올라갈경우.
            foreach (var e in Delegator<Delegatables.Process>.Keys)
            {
                Delegator<Delegatables.Process>.Get(e).Delegate(notifier.From, notifier.To, code, stream,
                    (Stream ret) =>
                    {
                        notifier.Response(code, ret);
                    },
                    () =>
                    {
                        notifier.Response(code, stream);
                    });
            }

        }
    }
}
