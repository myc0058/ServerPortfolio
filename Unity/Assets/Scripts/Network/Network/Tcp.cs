using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.IO.Compression;
using static Schema.Protobuf.Api;
using UnityEngine;

namespace Schema.Protobuf
{
    public class Tcp
    {
        public Aes aesAlg = null;
        public bool UseCompress { get; set; } = false;
        public delegate int AsyncReadCallback(Tcp sender, MemoryStream stream);
        public delegate void AsyncConnectCallback(Tcp sender, bool ret);
        public delegate void AsyncAcceptCallback(Tcp sender, bool ret);
        public delegate void AsyncDisconnectCallback(Tcp sender);

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
        
        public bool Connect(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
            if (socket != null) return false;

            try
            {
                lock (this)
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(ip, port, ConnectComplete, null);
                    state = EState.Connecting;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Disconnect();
                Debug.LogError(ex.StackTrace);
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
                    OnDisconnect?.Invoke(this);
                }
                catch
                {

                }


            }
        }

        protected Queue<object> pendings = new Queue<object>();
        public bool Write(ISerializable msg)
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
                    case ISerializable serializable:
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
                        catch(Exception ex)
                        {
                            Disconnect();
                            Debug.LogError(ex.StackTrace);
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
                    Console.WriteLine(e);
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
                    Console.WriteLine(e);
                    Disconnect();
                    return false;
                }
            }

            return true;
        }

        static private void DefaultOnDisconncect(Tcp sender)
        {
        }
        static private void DefaultOnConnect(Tcp sender, bool ret)
        {
        }
        static private void DefaultOnAccept(Tcp sender, bool ret)
        {
        }

        static private int DefaultOnRead(Tcp sender, MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.End);

            return 0;
        }
        
        private void ConnectComplete(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
                lock (this)
                {
                    state = EState.Establish;
                    OnConnect(this, true);
                    if (sendBuffer == null) { flush(); }
                }
                
                offset = 0;
                socket.BeginReceive(recvstream, 0, RecvBufferSize, SocketFlags.None, new System.AsyncCallback(RecvComplete), null);
                
                return;
            }
            catch
            {
                //Debug.LogError(ex.StackTrace);
            }
            state = EState.Closed;
            OnConnect(this, false);
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
                    Console.WriteLine($"blockSize < 1 || blockSize > RecvBufferSize, {blockSize} < 1 || {blockSize} > {RecvBufferSize}");
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
                    OnRead?.Invoke(this, result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("OnRead Exception " + e);
                }
                

            }

            transferred.Seek(readBytes, SeekOrigin.Begin);

        }

        private void RecvComplete(IAsyncResult ar)
        {
            SocketError error;
            try
            {
                int len = (int)socket.EndReceive(ar, out error);
                if (len == 0)
                {
                    Disconnect();
                    return;
                }

                len = offset + len;

                MemoryStream transferred = new MemoryStream(recvstream, 0, len, true, true);
                Defragment(transferred);
                offset = (int)len - (int)transferred.Position;
                if (offset < 0)
                {
                    Disconnect();
                    return;
                }

                Array.Copy(recvstream, transferred.Position, recvstream, 0, offset);
                socket.BeginReceive(recvstream, offset, RecvBufferSize - offset, SocketFlags.None, new System.AsyncCallback(RecvComplete), null);
                return;

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.StackTrace);
            }

            Disconnect();
        }

        protected void SendComplete(IAsyncResult ar)
        {
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
                catch (Exception ex)
                {
                    Disconnect();
                    Debug.LogError(ex.StackTrace);
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
