using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NLog.Logstash.Lumberjack
{
    class LumberjackProtocol : IProtocol
    {
        private static void WriteKvp(Stream stream, string key, string value)
        {
            var lenBuff = new byte[4];

            var dataBuff = Encoding.UTF8.GetBytes(key);
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

        public byte[] CreatePacket(IDictionary<string, object> data, int sequenceId)
        {
            using (var mem = new MemoryStream())
            {
                mem.WriteByte(1);
                mem.WriteByte((byte)'D');

                var buff = new byte[8];
                buff[0] = (byte)(sequenceId >> 24);
                buff[1] = (byte)(sequenceId >> 16);
                buff[2] = (byte)(sequenceId >> 8);
                buff[3] = (byte)(sequenceId);
                buff[7] = (byte)(data.Count);
                mem.Write(buff, 0, 8);

                foreach (var property in data)
                {
                    WriteKvp(mem, property.Key, property.Value as string ?? string.Empty);
                }
                return mem.ToArray();
            }
        }
    }
}
