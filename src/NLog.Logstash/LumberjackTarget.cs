using System;
using NLog.Common;
using NLog.Config;
using NLog.Logstash;
using NLog.Logstash.Lumberjack;


// ReSharper disable once CheckNamespace
namespace NLog.Targets
{
    [Target("Lumberjack")]
    public class LumberjackTarget : TargetBase
    {
        private TransportBase _transport;

        #region parameters
        [RequiredParameter]
        public string Host { get; set; }
        public int Port { get; set; } = 5000;
        [RequiredParameter]
        public string Thumbprint { get; set; }
        #endregion

        protected override IProtocol Protocol { get; set; } = new LumberjackProtocol();

        protected override void InitializeTarget()
        {
            _transport = new LumberjackTransport()
            {
                Host = Host,
                Port = Port,
                Thumbprint = Thumbprint,
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
