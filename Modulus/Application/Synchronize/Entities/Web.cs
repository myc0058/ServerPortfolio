using Engine.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Synchronize.Entities
{
    public partial class Web : Engine.Framework.Layer.Task
    {
        public class Notifier
        {
            public HttpListenerContext Context { get; set; }

            public void Response<T>(T msg)
            {
                var value = msg as Google.Protobuf.IMessage;
                var stream = value.ToMemoryStream();
                Context.Response.ContentLength64 = stream.Length;
                Context.Response.OutputStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
            }

            public void Response(byte[] bytes)
            {
                Context.Response.ContentLength64 = bytes.Length;
                Context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }

            public void Response(string value)
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                Context.Response.ContentLength64 = bytes.Length;
                Context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
        }
        public class Layer : Engine.Framework.Layer { }
        public Web() : base(Singleton<Layer>.Instance)
        {

        }
    }
}
