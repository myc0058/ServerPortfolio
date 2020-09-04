using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocol
{
    public class Dummy : Engine.Network.Protocol.Tcp
    {

        public Dummy()
        {
            OnRead = onRead;
        }

        private int onRead(MemoryStream transferred)
        {

            int offset = 0;
            byte[] buffer = transferred.GetBuffer();
            while ((transferred.Length - offset) > sizeof(ushort))
            {

                ushort size = BitConverter.ToUInt16(buffer, offset);
                if (size > transferred.Length - offset)
                {
                    break;
                }


                int idx = BitConverter.ToInt32(buffer, offset + 2);
                uint code = BitConverter.ToUInt32(buffer, offset + 6);


                Console.WriteLine("Idx : " + idx);
                Console.WriteLine("Code : " + code);

                //if (code == Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Login>.Value)
                //{
                //    //Write(new Schema.Protobuf.Message.Synchronize.GetInfo() { Handle = 111 });
                //}
                //else if (code == Engine.Framework.Id<Schema.Protobuf.Message.Synchronize.GetInfo>.Value)
                //{
                //    var stream = new MemoryStream(buffer, offset + 10, size - 10, true, true);
                //    var cis = new Google.Protobuf.CodedInputStream(stream);

                //    var msg = new Schema.Protobuf.Message.Synchronize.GetInfo();
                //    msg.MergeFrom(cis);

                //    Console.WriteLine("Name : " + msg.Name);

                //    //Write(new Schema.Protobuf.Message.Synchronize.GetInfo() { Handle = 111 });
                //}


                offset += size;
            }

            transferred.Seek(offset, SeekOrigin.Begin);

            return 0;
        }

        public void Write<T>(T msg) where T : Google.Protobuf.IMessage
        {

            using (MemoryStream strem = ProtobufToMemoryStream(msg))
            {
                base.Write(strem);    
            }


        }

        static public MemoryStream ProtobufToMemoryStream<T>(T msg) where T : Google.Protobuf.IMessage
        {
            var stream = new MemoryStream(4096);
            BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true);
            bw.Write((ushort)0);
            bw.Write((int)0);
            bw.Write(Engine.Framework.Id<T>.Value);

            using (var co = new Google.Protobuf.CodedOutputStream(stream, true))
            {
                msg.WriteTo(co);
            }

            ushort len = (ushort)stream.Length;
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(len);
            bw.Seek(0, SeekOrigin.Begin);

            return stream;

        }
    }
}
