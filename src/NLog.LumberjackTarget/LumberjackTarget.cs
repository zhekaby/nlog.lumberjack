using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog.Common;
using Newtonsoft.Json;
using NLog.Config;


namespace NLog.Targets.Lumberjack
{
    [Target("Lumberjack")]
    public class LumberjackTarget : TargetWithLayout
    {
        private readonly LumberjackProtocol _protocol = new LumberjackProtocol();

        [RequiredParameter]
        public string Host
        {
            get
            {
                return _protocol.Host;
            }
            set
            {
                _protocol.Host = value;
            }
        }
        public int Port
        {
            get
            {
                return _protocol.Port;
            }
            set
            {
                _protocol.Port = value;
            }
        }

        [RequiredParameter]
        public string Thumbprint
        {
            get
            {
                return _protocol.Thumbprint;
            }
            set
            {
                _protocol.Thumbprint = value;
            }
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            WriteAsyncInternal(logEvent.LogEvent)
                .ContinueWith(t =>
                {
                    logEvent.Continuation(t.Exception);
                });
        }




        private async Task WriteAsyncInternal(LogEventInfo logEvent)
        {
            var data = logEvent.Properties["data"] as LumberjackMessageBase;
            var log = new Dictionary<string, object>
            {
                { "source", data.Source  },
                { "app_id", data.ApplicationId },
                { "component", data.Component },
            };

            if (!string.IsNullOrWhiteSpace(data.MachineName))
            {
                log["machine_name"] = data.MachineName.ToLowerInvariant();
            }

            {
                var message = data as LumberjackLogMessage;
                if (message != null)
                {
                    log["type"] = "logs";
                    log["id"] = Guid.NewGuid().ToString("N");
                    log["level"] = message.Level.Name;
                    log["line"] = message.Message;
                    log["props"] = JsonConvert.SerializeObject(new Dictionary<string, object>
                    {
                        { "fields", message.Fields },
                        { "tags", message.Tags }
                    });

                    await _protocol.SendDataFrameAsync(log, logEvent.SequenceID);
                    return;
                }
            }

            {
                var message = data as LumberjackMetricMessage;
                if (message != null)
                {
                    log["type"] = "metric";
                    if (!string.IsNullOrWhiteSpace(data.MachineName))
                    {
                        log["line"] = string.Format("{0}.{1}.{2}.{3}.{4} {5} {6}", data.Source, data.MachineName.ToLowerInvariant(), data.ApplicationId, data.Component, message.Name, message.Value, message.UnixTimestamp);
                    }
                    else
                    {
                        log["line"] = string.Format("{0}.{1}.{2}.{3} {4} {5}", data.Source, data.ApplicationId, data.Component, message.Name, message.Value, message.UnixTimestamp);
                    }
                    await _protocol.SendDataFrameAsync(log, logEvent.SequenceID);
                    return;
                }

                
            }

            {
                var message = data as LumberjackAlertMessage;
                if (message != null)
                {
                    log["type"] = "alert";
                    log["line"] = message.Text;
                    log["rule"] = message.RuleName;

                    await _protocol.SendDataFrameAsync(log, logEvent.SequenceID);
                    return;
                }
            }

            throw new NotSupportedException("Log type is not supported");

        }
    }
}
