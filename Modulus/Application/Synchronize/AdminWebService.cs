using Engine.Framework;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Application.Synchronize
{
    public static class AdminWebService
    {
        static HttpListener listener = new HttpListener();
        public static void Run()
        {
            listener.Prefixes.Add("http://*:5281/");
            listener.Start();


            ThreadPool.QueueUserWorkItem((o) =>
            {
                Engine.Framework.Api.Logger.Info("HttpListen  Started...." + 5281);
                try
                {
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                Response(ctx);

                            }
                            catch (Exception e)
                            {
                                Engine.Framework.Api.Logger.Info(e);

                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                            }
                        }, listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        private static void Response(HttpListenerContext ctx)
        {
            string[] codes = ctx.Request.RawUrl.Split('/');

            int code = codes[codes.Length - 1].ToInt32();

            try
            {
                var web = Engine.Framework.Api.Singleton<Entities.Web>.Instance;

                var callback = Schema.Protobuf.Api.Bind(web, new Entities.Web.Notifier() { Context = ctx }, code, ctx.Request.InputStream);

                web.PostMessage(callback);

                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 200;
            }
            catch
            {
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 500;
            }
        }
        private static void Response(HttpListenerContext ctx, int code, string responseMessage)
        {
            
            return;
        }

    }
}
