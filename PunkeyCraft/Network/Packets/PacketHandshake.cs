using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PunkeyCraft.Network.Packets
{
    // Server -> Client
    class PacketHandshake : Packet
    {
        public const byte PacketID = 0x02;
        public byte ProtocolVersion { get; private set; }
        public string Username { get; private set; }
        public string ServerHost { get; private set; }
        public int ServerPort { get; private set; }

        public PacketHandshake(byte[] data) : base(data)
        {
            Reader.ReadByte();
            ProtocolVersion = Reader.ReadByte();
            Username = Reader.ReadString();
            ServerHost = Reader.ReadString();
            ServerPort = Reader.ReadInt32();
        }
    }
}
