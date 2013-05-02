using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using PunkeyCraft.Network;
using PunkeyCraft.Network.Packets;
using PunkeyCraft.Configuration;

namespace PunkeyCraft
{
    class PunkeyCraft
    {
        private TCPServer mServer;

        public PunkeyCraft()
        {
            Config.Load();
            mServer = new TCPServer(IPAddress.Parse(Config.Singleton.IP), Config.Singleton.Port);
            mServer.Error += mServer_Error;
            mServer.Info += mServer_Info;
            mServer.ClientConnected += mServer_ClientConnected;
            mServer.ClientDisconnected += mServer_ClientDisconnected;
            mServer.DataReceived += mServer_DataReceived;
        }

        public void Start()
        {
            mServer.Start();
            while (mServer.IsRunning)
            {
                var cmd = Console.ReadLine();
                if (cmd.Equals("stop"))
                {
                    Stop();
                    return;
                }
            }
        }

        public void Stop()
        {
            mServer.Stop();
        }

        private void mServer_Error(TCPServerEventArgs e)
        {
            Console.WriteLine("[Error]: " + e.Message);
        }
        private void mServer_Info(TCPServerEventArgs e)
        {
            Console.WriteLine("[Info]: " + e.Message);
        }
        private void mServer_ClientConnected(TCPServerEventArgs e)
        {
            Console.WriteLine("[Client]: {0} connected", (e.Connection.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
        }
        private void mServer_ClientDisconnected(TCPServerEventArgs e)
        {
            Console.WriteLine("[Client]: {0} disconnected", (e.Connection.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
        }
        private void mServer_DataReceived(TCPServerEventArgs e)
        {
            Console.WriteLine("[Client]: {0} data received", (e.Connection.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
        }
    }
}
