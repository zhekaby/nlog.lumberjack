using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Targets.Lumberjack
{
    public static class LoggerExtensions
    {
        public static void Log(this NLog.Logger logger, LumberjackMessage message)
        {
            if (message == null || message.Source == null || message.ApplicationId == null || message.Component == null)
            {
                return;
            }

            var info = new LogEventInfo
            {
                Level = message.Level,
            };

            info.Properties.Add("data", message);
            logger.Log(typeof(LoggerExtensions), info);
        }
        public static void Measure(this NLog.Logger logger, LumberjackMetricMessage message)
        {
            if (message == null || message.Source == null || message.ApplicationId == null || message.Component == null)
            {
                return;
            }

            if(message.UnixTimestamp == 0)
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
        public static void Alert(this NLog.Logger logger, LumberjackAlertMessage message)
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

        public static LumberjackMessageBuilder Setup(this NLog.Logger logger, string source, string app_id, string component)
        {
            return new LumberjackMessageBuilder(logger, source, app_id, component);
        }
    }
}
