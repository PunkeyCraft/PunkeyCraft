using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PunkeyCraft.Network
{
    public class TCPServer
    {
        private Dictionary<Guid, Connection> mConnections = new Dictionary<Guid, Connection>();
        private TcpListener mTcpListener;
        private object mLockObj = new object();
        private IPAddress mIP;

        #region Properties
        public int ClientsConnected
        {
            get { return mConnections.Count; }
        }

        public int Port
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }
        #endregion

        #region Events

        public event TCPServerEventHandler Error;
        public event TCPServerEventHandler Info;
        public event TCPServerEventHandler ClientConnected;
        public event TCPServerEventHandler ClientDisconnected;
        public event TCPServerEventHandler DataReceived;

        private void OnError(TCPServerEventArgs e)
        {
            if (Error != null) Error.Invoke(e);
        }
        private void OnInfo(TCPServerEventArgs e)
        {
            if (Info != null) Info(e);
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

        public TCPServer(IPAddress ip, int port)
        {
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                throw new Exception("Invalid port");

            mIP = ip;
            Port = port;
            IsRunning = false;
        }

        public void Start()
        {
            if (IsRunning)
                throw new Exception("Server is already running");

            IsRunning = true;

            mTcpListener = new TcpListener(mIP, Port);
            mTcpListener.Start();
            mTcpListener.BeginAcceptTcpClient(AcceptClient, null);
            OnInfo(new TCPServerEventArgs("Server is running"));
        }

        public void Stop()
        {
            if (!IsRunning)
                throw new Exception("Server is not running");

            IsRunning = false;

            mTcpListener.Stop();
            lock (mLockObj)
            {
                foreach (Connection connection in mConnections.Values)
                    connection.Disconnect();
                mConnections.Clear();
            }
        }

        private void AcceptClient(IAsyncResult r)
        {
            try
            {
                var tcpClient = mTcpListener.EndAcceptTcpClient(r);
                if (!IsRunning || tcpClient == null)
                    return;

                var connection = new Connection();
                connection.ClientConnected += connection_ClientConnected;
                connection.ClientDisconnected += connection_ClientDisconnected;
                connection.DataReceived += connection_DataReceived;
                connection.Error += connection_Error;
                connection.Connect(tcpClient);

                lock (mLockObj)
                    mConnections.Add(connection.GUID, connection);

                if (IsRunning)
                    mTcpListener.BeginAcceptTcpClient(AcceptClient, null);
            }
            catch (Exception ex)
            {
                Stop();
                OnError(new TCPServerEventArgs("[Listener]: " + ex.Message + "\n" + ex.StackTrace));
            }
        }

        public void Send(Guid guid, byte[] data)
        {
            lock (mLockObj)
            {
                if (!mConnections.ContainsKey(guid))
                    return;

                mConnections[guid].Send(data);
            }
        }

        public void Broadcast(byte[] data)
        {
            lock (mLockObj)
            {
                foreach (Connection con in mConnections.Values)
                    con.Send(data);
            }
        }

        #region intern events

        private void connection_ClientConnected(TCPServerEventArgs e)
        {
            OnClientConnected(e);
        }

        private void connection_ClientDisconnected(TCPServerEventArgs e)
        {
            lock (mLockObj)
            {
                if (mConnections.ContainsKey(e.Connection.GUID))
                    mConnections.Remove(e.Connection.GUID);
            }
            OnClientDisconnected(e);
        }

        private void connection_DataReceived(TCPServerEventArgs e)
        {
            OnDataReceived(e);
        }

        private void connection_Error(TCPServerEventArgs e)
        {
            lock (mLockObj)
            {
                if (mConnections.ContainsKey(e.Connection.GUID))
                    mConnections.Remove(e.Connection.GUID);
            }
            OnError(e);
        }

        #endregion
    }
}
