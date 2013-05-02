using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PunkeyCraft.Network.Packets
{
    // Serve <-> Client
    class PacketKeepAlive : Packet
    {
        public const byte PacketID = 0x00;
        public int ID { get; private set; }

        public PacketKeepAlive(int id) : base()
        {
            Writer.Write(PacketID);
            Writer.Write(id);
        }

        public PacketKeepAlive(byte[] data) : base(data)
        {
            Reader.ReadByte();
            ID = Reader.ReadInt32();
        }
    }
}
