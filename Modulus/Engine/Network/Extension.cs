using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Network
{
    public static class Extension
    {
        public static string GetIp(this System.Net.EndPoint value)
        {
            try
            {
                return (value as System.Net.IPEndPoint).Address.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public static long ToInt64Address(this System.Net.EndPoint value)
        {
            var ip = (value as System.Net.IPEndPoint).Address.ToString();
            var port = (value as System.Net.IPEndPoint).Port;
            return Engine.Framework.Api.AddressToInt64(ip, (ushort)port);
        }

    }
}
