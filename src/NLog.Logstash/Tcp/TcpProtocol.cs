using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NLog.Logstash.Tcp
{
    class TcpProtocol : IProtocol
    {
        public byte[] CreatePacket(IDictionary<string, object> data, int sequenceId)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data) + "\n");
        }
    }
}
