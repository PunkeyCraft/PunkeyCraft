using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PunkeyCraft.Network.Packets
{
    // Serve <-> Client
    class PacketChatMessage : Packet
    {
        public const byte PacketID = 0x03;
        public string Message { get; private set; }

        public PacketChatMessage(string message) : base()
        {
            Writer.Write(PacketID);
            Writer.Write(message);
        }

        public PacketChatMessage(byte[] data) : base(data)
        {
            Reader.ReadByte();
            Message = Reader.ReadString();
        }
    }
}
