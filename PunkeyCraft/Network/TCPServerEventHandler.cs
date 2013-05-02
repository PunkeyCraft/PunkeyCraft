using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PunkeyCraft.Network
{
    public delegate void TCPServerEventHandler(TCPServerEventArgs eventArgs);

    public class TCPServerEventArgs
    {
        public TCPServerEventArgs(string message)
        {
            Connection = null;
            Buffer = null;
            Message = message;
        }

        public TCPServerEventArgs(Connection connection, string message)
        {
            Connection = connection;
            Buffer = null;
            Message = message;
        }

        public TCPServerEventArgs(Connection connection, byte[] buffer)
        {
            Connection = connection;
            Buffer = buffer;
            Message = null;
        }

        public TCPServerEventArgs(Connection connection)
        {
            Connection = connection;
            Buffer = null;
            Message = null;
        }

        #region Properties
        public Connection Connection
        {
            get;
            private set;
        }

        public byte[] Buffer
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }
        #endregion
    }
}
