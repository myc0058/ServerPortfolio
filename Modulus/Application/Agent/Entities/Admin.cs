using Engine.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Agent.Entities
{
    // 에이전트 서버와 연결된 운영툴.
    public partial class Admin : Engine.Framework.Layer.Task
    {
        public class Notifier : Engine.Framework.INotifier
        {
            public HttpListenerContext Context { get; set; }

            public void Response<T>(T msg)
            {
                var value = msg as Google.Protobuf.IMessage;
                var stream = value.ToMemoryStream();
                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode = 200;
                Context.Response.ContentLength64 = stream.Length;
                Context.Response.OutputStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
                Context.Response.OutputStream.Flush();
                Context.Response.Close();
            }

            public void Response(byte[] bytes)
            {
                Context.Response.ContentLength64 = bytes.Length;
                Context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                Context.Response.Close();
            }

            public void Response(string value)
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                Context.Response.ContentLength64 = bytes.Length;
                Context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                Context.Response.Close();
            }

            public void Notify<T>(T  msg)
            {

            }
        }

    }
}
