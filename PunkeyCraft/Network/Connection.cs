using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PunkeyCraft.Network
{
    public class Connection
    {
        private static readonly int BufferSize = 10240; // 10 KB

        private TcpClient mTcpClient;
        private byte[] mBuffer;

        #region Events

        public event TCPServerEventHandler Error;
        public event TCPServerEventHandler ClientConnected;
        public event TCPServerEventHandler ClientDisconnected;
        public event TCPServerEventHandler DataReceived;

        private void OnError(TCPServerEventArgs e)
        {
            if (Error != null) Error.Invoke(e);
        }
        private void OnClientConnected(TCPServerEventArgs e)
        {
            if (ClientConnected != null) ClientConnected.Invoke(e);
        }
        private void OnClientDisconnected(TCPServerEventArgs e)
        {
            if (ClientDisconnected != null) ClientDisconnected.Invoke(e);
        }
        private void OnDataReceived(TCPServerEventArgs e)
        {
            if (DataReceived != null) DataReceived.Invoke(e);
        }

        #endregion

        #region Properties
        public Guid GUID
        {
            get;
            private set;
        }

        public bool IsConnected
        {
            get;
            private set;
        }

        public TcpClient TcpClient
        {
            get { return mTcpClient; }
            private set { }
        }
        #endregion

        public Connection()
        {
            GUID = Guid.NewGuid();
            IsConnected = false;
        }

        public void Connect(TcpClient tcpClient)
        {
            if (tcpClient == null || !tcpClient.Connected)
            {
                OnError(new TCPServerEventArgs(this, "Invalid TcpClient, NULL or not connected"));
                throw new Exception("Invalid TcpClient! NULL or not connected");
            }

            mTcpClient = tcpClient;
            mBuffer = new byte[BufferSize];
            mTcpClient.GetStream().BeginRead(mBuffer, 0, BufferSize, new AsyncCallback(Receiver), null);
            IsConnected = true;
            OnClientConnected(new TCPServerEventArgs(this));
        }

        private void Receiver(IAsyncResult r)
        {
            try
            {
                NetworkStream ns = mTcpClient.GetStream();
                int bytesReceived = 0;
                bytesReceived = ns.EndRead(r);
                if (bytesReceived == 0) // Connection closed
                {
                    Disconnect();
                    return;
                }

                byte[] tmp = new byte[bytesReceived];
                Array.Copy(mBuffer, tmp, bytesReceived);
                OnDataReceived(new TCPServerEventArgs(this, tmp));
                tmp = null;
                mBuffer = new byte[BufferSize];
                GC.Collect();
                if (IsConnected)
                    ns.BeginRead(mBuffer, 0, BufferSize, new AsyncCallback(Receiver), null);
            }
            catch (ObjectDisposedException) { Disconnect(); }
            catch (NullReferenceException) { Disconnect(); }
            catch (InvalidOperationException) { Disconnect(); }
            catch (Exception ex)
            {
                Disconnect();
                OnError(new TCPServerEventArgs(this, ex.Message + "\n" + ex.StackTrace));
            }
        }

        public void Send(byte[] buf)
        {
            if (!IsConnected)
                return;
            var ns = mTcpClient.GetStream();
            ns.Write(buf, 0, buf.Length);
            ns.Flush();
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            IsConnected = false;
            mBuffer = null;

            try
            {
                if (mTcpClient != null)
                {
                    mTcpClient.Client.Close();
                    mTcpClient.Client.Dispose();
                    mTcpClient.Close();
                    mTcpClient = null;
                }
            }
            catch (Exception ex) { }

            OnClientDisconnected(new TCPServerEventArgs(this));
        }
    }
}
