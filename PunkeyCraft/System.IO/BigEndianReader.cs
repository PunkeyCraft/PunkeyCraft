using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    class BigEndianReader : BinaryReader
    {
        public BigEndianReader(Stream stream) : base(stream)
        { }

        private byte[] EnsureEndian(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return data;
        }

        #region Read methods
        public override short ReadInt16()
        {
            return BitConverter.ToInt16(EnsureEndian(base.ReadBytes(2)), 0);
        }
        public override int ReadInt32()
        {
            return BitConverter.ToInt32(EnsureEndian(base.ReadBytes(4)), 0);
        }
        public override long ReadInt64()
        {
            return BitConverter.ToInt64(EnsureEndian(base.ReadBytes(8)), 0);
        }

        public override ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(EnsureEndian(base.ReadBytes(2)), 0);
        }
        public override uint ReadUInt32()
        {
            return BitConverter.ToUInt32(EnsureEndian(base.ReadBytes(4)), 0);
        }
        public override ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(EnsureEndian(base.ReadBytes(8)), 0);
        }

        public override float ReadSingle()
        {
            return BitConverter.ToSingle(EnsureEndian(base.ReadBytes(4)), 0);
        }
        public override double ReadDouble()
        {
            return BitConverter.ToDouble(EnsureEndian(base.ReadBytes(8)), 0);
        }

        public string ReadString(Encoding encoding = null)
        {
            short size = BitConverter.ToInt16(EnsureEndian(base.ReadBytes(2)), 0);
            if (encoding == null)
                return Encoding.Default.GetString(this.ReadBytes(size));
            else
                return encoding.GetString(this.ReadBytes(size));
        }
        #endregion
    }
}
