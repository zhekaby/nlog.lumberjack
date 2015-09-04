using System;
using NLog.Common;
using NLog.Config;
using NLog.Logstash;
using NLog.Logstash.Tcp;

namespace NLog.Targets
{
    [Target("LogstashTcp")]
    class TcpTarget : TargetBase
    {
        private TransportBase _transport;

        #region parameters
        [RequiredParameter]
        public string Host { get; set; }
        public int Port { get; set; } = 5000;
        #endregion

        protected override IProtocol Protocol { get; set; } = new TcpProtocol();

        protected override void InitializeTarget()
        {
            _transport = new TcpTransport()
            {
                Host = Host,
                Port = Port,
            };
        }

        protected override void CloseTarget()
        {
            _transport?.Dispose();
            _transport = null;
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            var packet = CreatePacket(logEvent.LogEvent);
            _transport.Push(packet);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var packet = CreatePacket(logEvent);
            _transport.Push(packet);
        }

        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            Array.ForEach(logEvents, Write);
        }
    }
}
