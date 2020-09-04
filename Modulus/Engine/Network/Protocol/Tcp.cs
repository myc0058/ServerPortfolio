using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Collections;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Engine.Network.Protocol
{
    public class Tcp
    {
        public Aes aesAlg = null;
        public bool UseCompress { get; set; } = false;
        public delegate int AsyncReadCallback(MemoryStream stream);
        public delegate void AsyncConnectCallback(bool ret);
        public delegate void AsyncAcceptCallback(bool ret);
        public delegate void AsyncDisconnectCallback();

        private AsyncReadCallback onRead = DefaultOnRead;
        private AsyncConnectCallback onConnect = DefaultOnConnect;
        private AsyncAcceptCallback onAccept = DefaultOnAccept;
        private AsyncDisconnectCallback onDistonnect = DefaultOnDisconncect;
        public string IP => ip;
        public ushort Port => port;
        protected string ip { get; set; }
        protected ushort port { get; set; }
        public AsyncReadCallback OnRead
        {
            set { onRead = value; }
            get { return onRead; }
        }
        public AsyncConnectCallback OnConnect
        {
            set { onConnect = value; }
            get { return onConnect; }
        }
        public AsyncAcceptCallback OnAccept
        {
            set { onAccept = value; }
            get { return onAccept; }
        }
        public AsyncDisconnectCallback OnDisconnect
        {
            set { onDistonnect = value; }
            get { return onDistonnect; }
        }
        public int SendBufferSize = 65535;
        public int RecvBufferSize {
            get { return recvBufferSize; }
            set
            {
                recvBufferSize = value;
                recvstream = new byte[value];
            }
        }

        private int recvBufferSize = 65535;
        protected byte[] sendBuffer = null;


        protected enum EState
        {
            Idle = 0,
            Connecting = 1,
            Establish = 2,
            Closed = 3,
        }


        protected EState state = EState.Idle;

        public bool Accept(ushort port)
        {
            if (socket != null)
            {
                Engine.Framework.Api.Logger.Info("!!!!!!!!!!!!!!! Accept Listen Fail socket == null !!!!!!!!!!!!!!");
                return false;
            }
            if (Engine.Network.Api.IsOpen == false) return false;

            this.port = port;

            Socket listen = Engine.Network.Api.Acceptor(port);
            if (listen == null)
            {
                Engine.Framework.Api.Logger.Info("!!!!!!!!!!!!!!! Accept Listen Fail listen == null !!!!!!!!!!!!!!");
                return false;
            }

            try
            {
                listen.BeginAccept(new AsyncCallback(ListenComplete), listen);
                return true;
            }
            catch (Exception e)
            {
                Engine.Framework.Api.Logger.Error("!!!!!!!!!!!!!!! Accept Listen Fail !!!!!!!!!!!!!!\n" + e);
            }
            return false;
        }
        public bool Connect(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
            if (socket != null) return false;
            if (Engine.Network.Api.IsOpen == false) return false;


            try
            {
                lock (this)
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    state = EState.Connecting;
                    socket.BeginConnect(ip, port, ConnectComplete, null);
                }

                return true;
            }
            catch (Exception ex)
            {
                Disconnect();
                Engine.Framework.Api.Logger.Error($"Connect Fail Ip : {this.ip}, Port : {this.port}");
            }
            return false;
        }

        public bool IsEstablish()
        {
            return state == EState.Establish;
        }

        public virtual bool IsClosed()
        {
            lock (this)
            {
                if (state == EState.Establish || state == EState.Connecting) { return false; }
                return true;
            }
        }

        public void Disconnect()
        {

            lock (this)
            {
                state = EState.Closed;
                if (socket == null) { return; }
                try
                {
                    socket.Close(0);
                    sendBuffer = null;
                    pendings.Clear();
                }
                catch
                {

                }
                finally
                {

                    socket = null;
                }

                try
                {
                    OnDisconnect?.Invoke();
                }
                catch
                {

                }


            }
        }

        protected Queue<object> pendings = new Queue<object>();
        public bool Write(Engine.Framework.ISerializable msg)
        {


            lock (this)
            {


                if (IsClosed()) return false;
                pendings.Enqueue(msg);
                if (sendBuffer != null) { return true; }
                try
                {
                    flush();
                }
                catch
                {
                    Disconnect();
                    return false;
                }

            }


            //MemoryStream stream = new MemoryStream();
            //msg.Serialize(stream);

            //stream.Seek(0, SeekOrigin.Begin);
            //Write(stream);
            // msg.s


            return true;
        }

        protected virtual void flush()
        {
            if (pendings.Count == 0) { return; }
            if (state != EState.Establish) { return; }
            MemoryStream output = new MemoryStream();
            output.Write(BitConverter.GetBytes((int)0), 0, 4);
            output.Seek(4, SeekOrigin.Begin);
            CryptoStream csEncrypt = null;
            Stream stream = output;


            if (aesAlg != null)
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                csEncrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
                stream = csEncrypt;
            }

            GZipStream compressionStream = null;
            if (UseCompress)
            {
                compressionStream = new GZipStream(stream, CompressionMode.Compress, true);
                stream = compressionStream;
            }


            int length = 0;
            while (pendings.Count > 0 && length < RecvBufferSize)
            {
                var msg = pendings.Dequeue();
                switch (msg)
                {
                    case Engine.Framework.ISerializable serializable:
                        length += serializable.Length;
                        serializable.Serialize(stream);
                        break;
                    case MemoryStream ms:
                        try
                        {
                            length += (int)ms.Length;
                            ms.CopyTo(stream);
                            //stream.Write(ms.GetBuffer(), 0, (int)ms.Length);
                        }
                        catch (Exception e)
                        {
                            Disconnect();
                        }
                        break;
                    case byte[] array:
                        length += array.Length;
                        stream.Write(array, 0, array.Length);
                        break;
                    default:
                        break;
                }
            }


            if (compressionStream != null)
            {
                compressionStream.Flush();
                compressionStream.Dispose();
            }

            if (csEncrypt != null)
            {
                csEncrypt.FlushFinalBlock();
            }




            if (output.Length == 2)
            {
                return;
            }

            output.Seek(0, SeekOrigin.Begin);
            output.Write(BitConverter.GetBytes((int)output.Length), 0, 4);
            output.Seek(0, SeekOrigin.Begin);

            sendBuffer = output.ToArray();

            if (csEncrypt != null)
            {
                csEncrypt.Dispose();
            }

            output.Dispose();

            if (sendBuffer == null || sendBuffer.Length == 0)
            {
                sendBuffer = null;
                return;
            }

            socket.BeginSend(sendBuffer, 0, (int)sendBuffer.Length, SocketFlags.None, SendComplete, null);
        }
        public bool Write(MemoryStream stream)
        {

            if (stream.Length == 0) return true;
            lock (this)
            {

                if (IsClosed()) return false;

                stream.Seek(0, SeekOrigin.Begin);
                pendings.Enqueue(stream);

                if (sendBuffer != null) {
                    return true;
                }

                try
                {
                    flush();
                }
                catch (Exception e)
                {
                    Engine.Framework.Api.Logger.Info(e);
                    Disconnect();
                    return false;
                }
            }

            return true;
        }
        public bool Write(MemoryStream header, MemoryStream body)
        {

            if (header.Length == 0) return true;
            lock (this)
            {

                if (IsClosed()) return false;

                header.Seek(0, SeekOrigin.Begin);
                pendings.Enqueue(header);
                if (body.Length > 0)
                {
                    body.Seek(0, SeekOrigin.Begin);
                    pendings.Enqueue(body);
                }

                if (sendBuffer != null) {
                    return true;
                }

                try
                {
                    flush();
                }
                catch (Exception e)
                {
                    Engine.Framework.Api.Logger.Info(e);
                    Disconnect();
                    return false;
                }
            }

            return true;
        }

        static private void DefaultOnDisconncect()
        {
        }
        static private void DefaultOnConnect(bool ret)
        {
        }
        static private void DefaultOnAccept(bool ret)
        {
        }

        static private int DefaultOnRead(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.End);

            return 0;
        }

        private void ListenComplete(IAsyncResult ar)
        {

            Socket listen = (Socket)ar.AsyncState;

            try
            {
                socket = listen.EndAccept(ar);

                lock (this)
                {
                    state = EState.Establish;
                    OnAccept(true);
                }
                socket.BeginReceive(recvstream, 0, RecvBufferSize, SocketFlags.None, new AsyncCallback(RecvComplete), null);

                return;
            }
            catch (Exception e)
            {
                state = EState.Closed;
                try
                {
                    OnAccept(false);
                }
                catch
                {

                }
                finally
                {
                    Disconnect();
                }
            }
            finally
            {
                Engine.Network.Api.Listen(port);
            }




        }
        private void ConnectComplete(IAsyncResult ar)
        {

            try
            {
                socket.EndConnect(ar);
                lock (this)
                {
                    state = EState.Establish;
                    OnConnect(true);
                    if (sendBuffer == null) { flush(); }
                }

                offset = 0;
                socket.BeginReceive(recvstream, 0, RecvBufferSize, SocketFlags.None, new AsyncCallback(RecvComplete), null);

                return;
            }
            catch (Exception e)
            {
                //myc0058
            }
            state = EState.Closed;
            OnConnect(false);
            Disconnect();

        }

        protected virtual void Defragment(MemoryStream transferred)
        {
            var buffer = transferred.GetBuffer();

            int blockSize = 0;
            int readBytes = 0;

            while (transferred.Length - readBytes > sizeof(int))
            {
                blockSize = BitConverter.ToInt32(buffer, readBytes);
                if (blockSize < 1 || blockSize > RecvBufferSize)
                {
                    Engine.Framework.Api.Logger.Info($"blockSize < 1 || blockSize > RecvBufferSize, {blockSize} < 1 || {blockSize} > {RecvBufferSize}");
                    transferred.Seek(transferred.Length, SeekOrigin.Begin);
                    Disconnect();
                    return;
                }

                if (blockSize + readBytes > transferred.Length) { break; }

                Stream stream = new MemoryStream(buffer, readBytes + 4, blockSize - 4, true, true);
                readBytes += blockSize;


                CryptoStream csEncrypt = null;
                if (aesAlg != null)
                {
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    csEncrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
                    stream = csEncrypt;
                }

                GZipStream compressionStream = null;
                if (UseCompress)
                {
                    compressionStream = new GZipStream(stream, CompressionMode.Decompress, true);
                    stream = compressionStream;
                }


                MemoryStream result = new MemoryStream();

                stream.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);

                try
                {
                    OnRead?.Invoke(result);
                }
                catch (Exception e)
                {
                    Engine.Framework.Api.Logger.Info("OnRead Exception " + e);
                }


            }

            transferred.Seek(readBytes, SeekOrigin.Begin);

        }

        //internal void Merge(MemoryStream force, Tcp protocol)
        //{
        //    if (protocol == this)
        //    {
        //        lock (this)
        //        {
        //            while (pendings.Count > 0)
        //            {
        //                sendings.Enqueue(pendings.Dequeue());
        //            }

        //            pendings.Enqueue(force);

        //            while (sendings.Count > 0)
        //            {
        //                pendings.Enqueue(sendings.Dequeue());
        //            }
        //        }
        //        return;
        //    }

        //    Queue<object> sending = null;
        //    lock (protocol)
        //    {
        //        sendings = protocol.sendings;
        //        protocol.sendings = null;

        //        while (protocol.pendings.Count > 0)
        //        {
        //            sendings.Enqueue(protocol.pendings.Dequeue());
        //        }
        //    }
        //    lock (this)
        //    {
        //        if (sendings != null)
        //        {
        //            while (sendings.Count > 0)
        //            {
        //                pendings.Enqueue(sending.Dequeue());
        //            }
        //        }
        //    }
        //}

        //internal void Merge(Tcp protocol)
        //{
        //    if (protocol == null) { return; }
        //    if (protocol == this)
        //    {
        //        lock (this)
        //        {
        //            while(pendings.Count > 0)
        //            {
        //                sendings.Enqueue(pendings.Dequeue());
        //            }

        //            while (sendings.Count > 0)
        //            {
        //                pendings.Enqueue(sendings.Dequeue());
        //            }
        //        }
        //        return;
        //    }

        //    Queue<object> preSendings = null;
        //    lock (protocol)
        //    {
        //        preSendings = protocol.sendings;
        //        protocol.sendings = new Queue<object>();


        //        while (protocol.pendings.Count > 0)
        //        {
        //            preSendings.Enqueue(protocol.pendings.Dequeue());
        //        }
        //    }
        //    lock (this)
        //    {
        //        if (preSendings != null && preSendings.Count > 0)
        //        {
        //            var prePending = pendings;
        //            pendings = new Queue<object>();
        //            while(preSendings.Count > 0)
        //            {
        //                pendings.Enqueue(preSendings.Dequeue());
        //            }

        //            while (prePending.Count > 0)
        //            {
        //                pendings.Enqueue(prePending.Dequeue());
        //            }
        //        }
        //    }
        //}

        private ThreadLocal<Stopwatch> stopWatch = new ThreadLocal<Stopwatch>(() => { return new Stopwatch(); });
        private void RecvComplete(IAsyncResult ar)
        {
            //Engine.Framework.Api.Logger.Info($"RecvComplete now : {DateTime.UtcNow.Ticks / 10000}");
            SocketError error;
            try
            {
                stopWatch.Value.Start();
                int len = (int)socket.EndReceive(ar, out error);
                if (len == 0)
                {
                    Disconnect();
                    return;
                }

                len = offset + len;

                MemoryStream transferred = new MemoryStream(recvstream, 0, len, true, true);
                Defragment(transferred);
                stopWatch.Value.Stop();
                //Engine.Framework.Api.Logger.Info($"Defragment processing time : {stopWatch.Value.ElapsedTicks / 10000}");

                offset = (int)len - (int)transferred.Position;
                if (offset < 0)
                {
                    Disconnect();
                    return;
                }

                Array.Copy(recvstream, transferred.Position, recvstream, 0, offset);
                socket.BeginReceive(recvstream, offset, RecvBufferSize - offset, SocketFlags.None, new AsyncCallback(RecvComplete), null);
                return;

            }
            catch (Exception e)
            {

            }

            Disconnect();



        }

        protected void SendComplete(IAsyncResult ar)
        {
            //Engine.Framework.Api.Logger.Info($"SendComplete now : {DateTime.UtcNow.Ticks / 10000}");
            lock (this)
            {
                try
                {
                    int len = socket.EndSend(ar);
                    if (len == 0)
                    {
                        Disconnect();
                        return;
                    }
                    sendBuffer = null;
                    if (pendings.Count > 0)
                    {
                        flush();
                    }

                    return;

                }
                catch (Exception e)
                {
                    Disconnect();
                }
            }

        }

        protected Socket socket = null;
        int offset = 0;
        private byte[] recvstream = new byte[65535];

        public EndPoint RemoteEndPoint
        {
            get
            {
                try
                {
                    return socket.RemoteEndPoint;
                }
                catch
                {
                    return null;
                }
            }
        }
        public EndPoint LocalEndPoint
        {
            get
            {
                try
                {
                    return socket.LocalEndPoint;
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
