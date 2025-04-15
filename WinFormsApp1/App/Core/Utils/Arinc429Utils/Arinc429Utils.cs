using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.Utils.Arinc429Utils
{
    public class Arinc429Utils
    {
        public class Arinc429Message
        {
            private uint rawMessage = 0;

            /// <summary>
            /// Data field of the Arinc 429 message
            /// </summary>
            public uint Data { get => (rawMessage >> 3) & 0x3FFFF; }

            /// <summary>
            /// Label of the Arinc 429 message
            /// </summary>
            public byte Label { get => (byte)((rawMessage >> 24) & 0xFF); }

            /// <summary>
            /// SSM field of the Arinc 429 message
            /// </summary>
            public byte SSM { get => (byte)((rawMessage >> 1) & 0x03);  }

            /// <summary>
            /// The SDI field of the ARINC 429 message
            /// </summary>
            public byte SDI { get => (byte)((rawMessage >> 22) & 0x03);  }

            public Arinc429Message(byte[] data) {

                if (data.Length != 4)
                {
                    throw new ArgumentException($"Expected byte array with length 4, got {data.Length}");
                }

                rawMessage |= data[3];
                rawMessage |= (uint)data[2] << 8;
                rawMessage |= (uint)data[1] << 16;
                rawMessage |= (uint)data[0] << 24;
            }
        }
    }
}
