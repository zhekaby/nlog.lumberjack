using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLog.Targets.Lumberjack
{
    class LumberjackProtocol
    {
        private void WriteKVP(Stream stream, string key, string value)
        {
            var lenBuff = new byte[4];
            byte[] dataBuff;

            dataBuff = Encoding.UTF8.GetBytes(key);
            lenBuff[0] = (byte)(dataBuff.Length >> 24);
            lenBuff[1] = (byte)(dataBuff.Length >> 16);
            lenBuff[2] = (byte)(dataBuff.Length >> 8);
            lenBuff[3] = (byte)(dataBuff.Length);
            stream.Write(lenBuff, 0, lenBuff.Length);
            stream.Write(dataBuff, 0, dataBuff.Length);

            dataBuff = Encoding.UTF8.GetBytes(value);
            lenBuff[0] = (byte)(dataBuff.Length >> 24);
            lenBuff[1] = (byte)(dataBuff.Length >> 16);
            lenBuff[2] = (byte)(dataBuff.Length >> 8);
            lenBuff[3] = (byte)(dataBuff.Length);
            stream.Write(lenBuff, 0, lenBuff.Length);
            stream.Write(dataBuff, 0, dataBuff.Length);
        }

        internal byte[] CreatePacket(Dictionary<string, object> data, int sequenceID)
        {
            using (var mem = new MemoryStream())
            {
                mem.WriteByte(1);
                mem.WriteByte((byte)'D');

                var buff = new byte[8];
                buff[0] = (byte)(sequenceID >> 24);
                buff[1] = (byte)(sequenceID >> 16);
                buff[2] = (byte)(sequenceID >> 8);
                buff[3] = (byte)(sequenceID);
                buff[7] = (byte)(data.Count);
                mem.Write(buff, 0, 8);

                foreach (var property in data)
                {
                    WriteKVP(mem, property.Key, property.Value as string ?? string.Empty);
                }
                return mem.ToArray();
            }
        }
    }
}
