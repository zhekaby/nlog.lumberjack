using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NLog.Targets;

namespace NLog.Logstash
{
    public interface IProtocol
    {
        byte[] CreatePacket(IDictionary<string, object> data, int sequenceId);
    }

    public abstract class TargetBase : TargetWithLayout
    {
        protected abstract IProtocol Protocol { get; set; }

        protected byte[] CreatePacket(LogEventInfo logEvent)
        {
            var data = logEvent.Properties["data"] as LogstashMessageBase;

            if (data == null)
                throw new ArgumentException("Incorrect target usage");

            var dt = DateTime.UtcNow;
            var log = new Dictionary<string, object>
            {
                { "source", data.Source  },
                { "app_id", data.ApplicationId },
                { "component", data.Component },
                { "ts", dt.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture) + "Z" },
                { "date", dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) }
            };

            if (!string.IsNullOrWhiteSpace(data.MachineName))
            {
                log["machine_name"] = data.MachineName.ToLowerInvariant();
            }

            {
                var message = data as LogstashMessage;
                if (message != null)
                {
                    log["type"] = "logs";
                    log["level"] = message.Level.Name;
                    log["line"] = message.Message;
                    log["props"] = new Dictionary<string, object>
                    {
                        { "fields", message.Fields },
                        { "tags", message.Tags }
                    };

                    return Protocol.CreatePacket(log, logEvent.SequenceID);
                }
            }

            {
                var message = data as LogstashMetricMessage;
                if (message != null)
                {
                    log["type"] = "metric";
                    if (!string.IsNullOrWhiteSpace(data.MachineName))
                    {
                        log["line"] =
                            $"{data.Source}.{data.MachineName.ToLowerInvariant()}.{data.ApplicationId}.{data.Component}.{message.Name} {message.Value} {message.UnixTimestamp}";
                    }
                    else
                    {
                        log["line"] =
                            $"{data.Source}.{data.ApplicationId}.{data.Component}.{message.Name} {message.Value} {message.UnixTimestamp}";
                    }
                    return Protocol.CreatePacket(log, logEvent.SequenceID);
                }


            }

            {
                var message = data as LogstashAlertMessage;

                if (message == null)
                    throw new NotSupportedException("Log type is not supported");

                log["type"] = "alert";
                log["line"] = message.Text;
                log["rule"] = message.RuleName;

                return Protocol.CreatePacket(log, logEvent.SequenceID);
            }

            throw new NotSupportedException("Log type is not supported");
        }
    }
}
