using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    class BigEndianWriter : BinaryWriter
    {
        public BigEndianWriter(Stream stream) : base(stream)
        { }

        private byte[] EnsureEndian(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return data;
        }

        #region Write methods
        public override void Write(Int16 val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(Int32 val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(long val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }

        public override void Write(UInt16 val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(UInt32 val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(UInt64 val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }

        public override void Write(float val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(double val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(long val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(short val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }
        public override void Write(Single val)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes(val)));
        }

        public void Write(string val, Encoding encoding = null)
        {
            base.Write(EnsureEndian(BitConverter.GetBytes((UInt16)val.Length)));
            if (encoding == null)
                base.Write(Encoding.ASCII.GetBytes(val));
            else
                base.Write(encoding.GetBytes(val));
        }
        #endregion
    }
}
