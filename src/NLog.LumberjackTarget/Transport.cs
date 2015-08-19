using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    internal class Transport : IDisposable
    {
        private SslStream stream;
        public string Host { get; set; }
        public int Port { get; set; } = 5000;
        public string Thumbprint { get; set; }
        public int QueueCapacity { get; set; } = 50000;

        private readonly ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();

        CancellationTokenSource cts;

        public void Send(byte[] packet)
        {
            if (queue.Count == QueueCapacity || packet == null)
            {
                return;
            }
            queue.Enqueue(packet);

            if (cts == null)
            {
                cts = new CancellationTokenSource();
                DoWork(cts.Token);
            }
        }

        private void DoWork(CancellationToken token)
        {
            Task.Run(async () =>
            {
                do
                {
                    byte[] packet;
                    while (queue.TryPeek(out packet))
                    {
                        try
                        {
                            await EnsureConnectedAsync();
                            await stream.WriteAsync(packet, 0, packet.Length);
                        }
                        catch
                        {
                            CloseStream();
                            break;
                        }
                        queue.TryDequeue(out packet);
                    }
                    await Task.Delay(1);
                } while (!token.IsCancellationRequested);
            });
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
            if (cts != null)
            {
                cts.Cancel();
            }
            CloseStream();
        }
    }
}
