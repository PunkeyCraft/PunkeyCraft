using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PunkeyCraft.Network.Packets
{
    // Server -> Client
    class PacketLoginRequest : Packet
    {
        public const byte PacketID = 0x01;

        public PacketLoginRequest(int entityID, string levelType, byte gameMode, byte dimension, byte difficulty, byte maxPlayers) : base()
        {
            Writer.Write(PacketID);
            Writer.Write(entityID);
            Writer.Write(levelType);
            Writer.Write(gameMode);
            Writer.Write(dimension);
            Writer.Write(difficulty);
            Writer.Write((byte)0); // Not used
            Writer.Write(maxPlayers);
        }
    }
}
