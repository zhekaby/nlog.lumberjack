using System;
using NLog.Fluent;
using NLog.Targets.Lumberjack.Builders;

namespace NLog.Targets.Lumberjack.Loggers
{
    public class NlogLogger : IMessageLogger
    {
        private ILogger logger;
        public NlogLogger(ILogger logger)
        {
            this.logger = logger;
        }
        public void LogMessage(LumberjackLogMessage message)
        {
            if (message == null || message.Source == null || message.ApplicationId == null || message.Component == null)
            {
                return;
            }

            var info = new LogEventInfo
            {
                Level = message.Level
            };

            info.Properties.Add("data", message);
            logger.Log(typeof(LoggerExtensions), info);
        }

        public void LogAlert(LumberjackAlertMessage message)
        {
            if (message == null || message.Source == null || message.ApplicationId == null || message.Component == null)
            {
                return;
            }

            var info = new LogEventInfo
            {
                Level = LogLevel.Trace,
            };

            info.Properties.Add("data", message);
            logger.Log(typeof(LoggerExtensions), info);
        }

        public void LogMeasure(LumberjackMetricMessage message)
        {
            if (message == null || message.Source == null || message.ApplicationId == null || message.Component == null)
            {
                return;
            }

            if (message.UnixTimestamp == 0)
            {
                message.UnixTimestamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            }

            var info = new LogEventInfo
            {
                Level = LogLevel.Trace,
            };

            info.Properties.Add("data", message);
            logger.Log(typeof(LoggerExtensions), info);
        }
    }
}
