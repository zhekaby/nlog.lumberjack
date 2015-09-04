using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NLog.Logstash.Lumberjack
{
    internal class LumberjackTransport : TransportBase
    {
        public string Thumbprint { get; set; }
        public string Host { get; set; }
        public int Port { get; set; } = 5000;

        protected override async Task<Stream> GetStream()
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(Host, Port);
            var stream = new SslStream(tcpClient.GetStream(), false, (source, cert, chain, policy)
                => Thumbprint.Equals(cert.GetCertHashString(), StringComparison.OrdinalIgnoreCase));
            await stream.AuthenticateAsClientAsync("", new X509CertificateCollection(), SslProtocols.Tls, true);

            return stream;
        }
    }
}
