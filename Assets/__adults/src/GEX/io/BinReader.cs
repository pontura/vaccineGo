#define STR_ALGO_VER1

using System;
using System.Collections.Generic;
using System.Text;

namespace GEX.io
{
    public class BinReader
    {
        public int index;
        private byte [] data;
        private bool littleEndian = true;

        public BinReader(byte [] data)
        {
            this.data = data;
            this.index = 0;
            this.data = data;
        }

        public byte readByte()
        {
            byte val = data[index];
            index += 1;
            return val;
        }

        public UInt16 readUInt16()
        {
            UInt16 val = BitConverter.ToUInt16(data, index);
            index += 2; 
            return val;
        }

        public UInt32 readUInt32()
        {
            UInt32 val = BitConverter.ToUInt32(data, index);
            index += 4;
            return val;
        }

        public Int16 readInt16()
        {
            Int16 val = BitConverter.ToInt16(data, index);
            index += 2;
            return val;
        }

        public Int32 readInt32()
        {
            Int32 val = BitConverter.ToInt16(data, index);
            index += 4;
            return val;
        }

        public float readFloat32()
        {
            float val = BitConverter.ToSingle(data, index);
            index += 4;
            return val;
        }

        public String readString()
        {
#if STR_ALGO_VER1
            String str = "";
            byte [] bytes = data;
            char c;

            while (index < bytes.Length)
            {
                c = (char)bytes[index++];
                if (c != 0x00)
                    str += c;
                else
                    break;
            }

            return str;
#else

            StringBuilder str = new StringBuilder();
            byte[] bytes = data;
            char c;

            while (index < bytes.Length)
            {
                c = (char)bytes[index++];
                if (c != 0x00)
                    str.Append(c);
                else
                    break;
            }

            return str.ToString();
#endif
        }
    }
}