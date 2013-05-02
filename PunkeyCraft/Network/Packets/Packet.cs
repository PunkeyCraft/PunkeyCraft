using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PunkeyCraft.Network.Packets
{
    class Packet : IDisposable
    {
        private bool mIsDisposed = false;

        public BigEndianWriter Writer { get; protected set; }
        public BigEndianReader Reader { get; protected set; }

        public Packet() 
        {
            Writer = new BigEndianWriter(new MemoryStream()); 
        }
        public Packet(byte[] data)
        {
            Reader = new BigEndianReader(new MemoryStream(data));
        }

        public byte[] GetData()
        {
            if (Writer == null)
                return null;
            return (Writer.BaseStream as MemoryStream).ToArray();
        }

        public void Dispose()
        {
            if (mIsDisposed)
                return;

            if (Writer != null)
            {
                Writer.Dispose();
                Writer = null;
            }
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }

            GC.SuppressFinalize(this);
            mIsDisposed = true;
        }
    }
}
