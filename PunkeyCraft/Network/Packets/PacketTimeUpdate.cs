using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PunkeyCraft.Network.Packets
{
    // Serve -> Client
    class PacketTimeUpdate : Packet
    {
        public const byte PacketID = 0x04;

        public PacketTimeUpdate(long worldTime, long dayTime) : base()
        {
            Writer.Write(PacketID);
            Writer.Write(worldTime);
            Writer.Write(dayTime);
        }
    }
}
