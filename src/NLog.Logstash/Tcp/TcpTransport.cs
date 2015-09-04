using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NLog.Logstash.Tcp
{
    class TcpTransport : TransportBase
    {
        public string Host { get; set; }
        public int Port { get; set; } = 5001;

        protected override Task<Stream> GetStream()
        {
            return Task.FromResult(new TcpClient(Host, Port).GetStream() as Stream);
        }
    }
}
