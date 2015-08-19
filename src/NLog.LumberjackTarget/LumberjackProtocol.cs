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
    class LumberjackProtocol : IDisposable
    {
        private SslStream stream;
        int counter;
        public string Host { get; set; }
        public int Port { get; set; } = 5000;
        public string Thumbprint { get; set; }

        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1);

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

        internal async Task SendDataFrameAsync(Dictionary<string, object> data, int sequenceID)
        {
            //Debug.WriteLine("Seq: " + sequenceID);
            byte[] bytes;
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
                bytes = mem.ToArray();
            }

            await _lock.WaitAsync();
            try
            {
                await EnsureConnectedAsync();
                await stream.WriteAsync(bytes, 0, bytes.Length);
                //Debug.WriteLine("Write: " + ++counter);
            }
            catch (IOException ex)
            {
                //TODO: save packet
                //Debug.WriteLine(ex);
                CloseStream();
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
                CloseStream();
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task EnsureConnectedAsync()
        {
            if (stream != null)
            {
                return;
            }

            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(Host, Port);
            stream = new SslStream(tcpClient.GetStream(), false, (source, cert, chain, policy) =>
            {
                return Thumbprint.Equals(cert.GetCertHashString(), StringComparison.OrdinalIgnoreCase);
            });
            await stream.AuthenticateAsClientAsync("", new X509CertificateCollection(), SslProtocols.Tls, true);
        }

        private void CloseStream()
        {
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        public void Dispose()
        {
            CloseStream();
        }
    }
}
