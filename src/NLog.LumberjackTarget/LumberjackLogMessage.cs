using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Targets.Lumberjack
{
    public abstract class LumberjackMessageBase
    {
        public LumberjackMessageBase(string source, string applicationId, string component)
        {
            Source = source;
            ApplicationId = applicationId;
            Component = component;
        }
        public string Source { get; set; }
        public string ApplicationId { get; set; }
        public string Component { get; set; }
        public string MachineName { get; set; }

    }
    public class LumberjackLogMessage : LumberjackMessageBase
    {
        public LumberjackLogMessage(string source, string applicationId, string component, LogLevel level, string message)
            : base(source, applicationId, component)
        {
            Level = level;
            Message = message;
        }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public HashSet<string> Tags { get; set; }
    }

    public class LumberjackMetricMessage : LumberjackMessageBase
    {
        public LumberjackMetricMessage(string source, string applicationId, string component, string name, long unixTimestamp, double value)
            : base(source, applicationId, component)
        {
            Name = name;
            UnixTimestamp = unixTimestamp;
            Value = value;
        }
        public double Value { get; set; }
        public long UnixTimestamp { get; set; }
        public string Name { get; set; }
    }

    public class LumberjackAlertMessage : LumberjackMessageBase
    {
        public LumberjackAlertMessage(string source, string applicationId, string component, string ruleName, string text)
             : base(source, applicationId, component)
        {
            Text = text;
            RuleName = ruleName;
        }
        public string Text { get; set; }
        public string RuleName { get; set; }
    }
}
