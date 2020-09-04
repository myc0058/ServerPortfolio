using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Engine.Framework.Google
{
    public class Receipt
    {

        public static void Initialize(string MY_BASE64_PUBLIC_KEY)
        {
            cryptoServiceProviderXml = PEMKeyLoader.CryptoServiceProviderFromPublicKeyInfo(MY_BASE64_PUBLIC_KEY).ToXmlString(false);
        }

        public static string cryptoServiceProviderXml = null;

        public class Result
        {
            public string orderId = String.Empty;
            public string packageName = String.Empty;
            public string productId = String.Empty;
            public string purchaseTime = String.Empty;
            public string purchaseState = String.Empty;
            public string developerPayload = String.Empty;
            public string purchaseToken = String.Empty;
        }

        public static bool Verify(string message, string base64Signature)
        {

            try
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.FromXmlString(cryptoServiceProviderXml);

                byte[] signature = Convert.FromBase64String(base64Signature);
                SHA1Managed sha = new SHA1Managed();
                byte[] data = Encoding.UTF8.GetBytes(message);

                bool result = provider.VerifyData(data, sha, signature);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
