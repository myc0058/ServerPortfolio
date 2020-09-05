using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
//using Newtonsoft.Json.Linq;

namespace Engine.Framework.Apple
{
    static public class Receipt
    {
        public class Result
        {
            //var str  = "{\"receipt\":{ \"original_purchase_date_pst\":\"2017-02-22 20:20:46 America/Los_Angeles\", \"purchase_date_ms\":\"1487823646055\", \"unique_identifier\":\"7b02a6a76426bfec077c65ff0235ed58b7208e0f\", \"original_transaction_id\":\"1000000276082080\", \"bvrs\":\"5\", \"transaction_id\":\"1000000276082080\", \"quantity\":\"1\", \"unique_vendor_identifier\":\"7072C350-A36C-4995-A165-03A240C8A0A5\", \"item_id\":\"1208126630\", \"product_id\":\"ttp_g0001_ios_g\", \"purchase_date\":\"2017-02-23 04:20:46 Etc/GMT\", \"original_purchase_date\":\"2017-02-23 04:20:46 Etc/GMT\", \"purchase_date_pst\":\"2017-02-22 20:20:46 America/Los_Angeles\", \"bid\":\"com.daerisoft.ttp.ios.global\", \"original_purchase_date_ms\":\"1487823646055\"}, \"status\":0}";
            //var  result = Newtonsoft.Json.JsonConvert.DeserializeObject<Engine.Framework.Apple.Receipt.Result>(str);

            [Serializable]
            public class Info
            {
                public string original_transaction_id;
                public string transaction_id;
                public string product_id;
                public string purchase_date;
                public string original_purchase_date;
                public string unique_identifier;
            }
            public Info receipt;
            public int status;
            public bool sandbox = false;

        }
        public static bool SandBox = false;

        public delegate void Callback(Result result);
        public static void VerifyAsync(string receiptData, Callback callback = null)
        {
            try
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {

                    try
                    {
                        bool sandbox = false;
                        int tryVerify = 0;
                        Engine.Framework.Apple.Receipt.Result result = new Engine.Framework.Apple.Receipt.Result();
                        result.status = -1;

                        while (tryVerify++ < 3)
                        {
                            try
                            {
                                // Verify the receipt with Apple
                                string postString = String.Format("{{ \"receipt-data\" : \"{0}\" }}", receiptData);
                                byte[] postBytes = Encoding.UTF8.GetBytes(postString);
                                HttpWebRequest request;

                                if (sandbox == true)
                                {
                                    request = WebRequest.Create("https://sandbox.itunes.apple.com/verifyReceipt") as HttpWebRequest;
                                }
                                else
                                {
                                    request = WebRequest.Create("https://buy.itunes.apple.com/verifyReceipt") as HttpWebRequest;
                                }



                                request.Method = "POST";
                                request.ContentType = "text/plain";
                                request.ContentLength = postBytes.Length;
                                using (Stream postStream = request.GetRequestStream())
                                {
                                    postStream.Write(postBytes, 0, postBytes.Length);
                                    postStream.Close();
                                }


                                using (WebResponse r = request.GetResponse())
                                {
                                    using (System.IO.StreamReader sr = new System.IO.StreamReader(r.GetResponseStream()))
                                    {
                                        var data = sr.ReadToEnd();
                                        try
                                        {
                                            result = Newtonsoft.Json.JsonConvert.DeserializeObject<Engine.Framework.Apple.Receipt.Result>(data);
                                        }
                                        catch (Exception)
                                        {

                                        }

                                    }
                                }

                                if (result.status == 21007 && sandbox == false)
                                {
                                    tryVerify = 0;
                                    sandbox = true;
                                    continue;
                                }

                                if (result.status == 0)
                                {
                                    break;
                                }

                            }
                            catch (Exception)
                            {
                                // We crashed and burned — do something intelligent
                            }
                        }

                        if (callback != null)
                        {
                            try
                            {
                                result.sandbox = sandbox;
                                callback(result);
                            }
                            catch
                            {

                            }

                        }

                    }
                    catch
                    {
                        if (callback != null)
                        {
                            try
                            {
                                Result result = new Result();
                                result.receipt = new Result.Info();
                                result.status = -1;
                                callback(result);
                            }
                            catch
                            {

                            }

                        }
                    }

                });
            }
            catch
            {
                if (callback != null)
                {
                    try
                    {
                        Result result = new Result();
                        result.receipt = new Result.Info();
                        result.status = -1;
                        callback(result);
                    }
                    catch
                    {

                    }
                    
                }
            }
            
        }

        public static string Verify(string receiptData, bool sandbox = false)
        {
            try
            {
                // Verify the receipt with Apple
                string postString = String.Format("{{ \"receipt-data\" : \"{0}\" }}", receiptData);
                byte[] postBytes = Encoding.UTF8.GetBytes(postString);
                HttpWebRequest request;

                if (sandbox == true)
                {
                    request = WebRequest.Create("https://sandbox.itunes.apple.com/verifyReceipt") as HttpWebRequest;
                }
                else
                {
                    request = WebRequest.Create("https://buy.itunes.apple.com/verifyReceipt") as HttpWebRequest;
                }



                request.Method = "POST";
                request.ContentType = "text/plain";
                request.ContentLength = postBytes.Length;
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(postBytes, 0, postBytes.Length);
                    postStream.Close();
                }


                using (WebResponse r = request.GetResponse())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(r.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
                }

                // Perform the purchase — all my purchases are server-side only, which is a very secure way of doing things

                // Success!
            }
            catch (Exception)
            {
                // We crashed and burned — do something intelligent
            }

            return string.Empty;

        }
    }
}
