using System.Collections.Generic;

namespace NLog.Logstash
{
    public abstract class LogstashMessageBase
    {
        protected LogstashMessageBase() { }

        protected LogstashMessageBase(string source, string applicationId, string component)
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

    public class LogstashMessage : LogstashMessageBase
    {
        public LogstashMessage(string source, string applicationId, string component, LogLevel level, string
             message)
            : base(source, applicationId, component)
        {
            Level = level;
            Message = message;
            Tags = new HashSet<string>();
        }

        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public HashSet<string> Tags { get; set; }
    }

    public class LogstashMetricMessage : LogstashMessageBase
    {
        public LogstashMetricMessage(string source, string applicationId, string component, string name, double value, long timestamp)
            : base(source, applicationId, component)
        {
            Name = name;
            Value = value;
            UnixTimestamp = timestamp;
        }
        public double Value { get; set; }
        public long UnixTimestamp { get; set; }
        public string Name { get; set; }
    }

    public class LogstashAlertMessage : LogstashMessageBase
    {
        public LogstashAlertMessage(string source, string applicationId, string component, string ruleName, string text)
             : base(source, applicationId, component)
        {
            Text = text;
            RuleName = ruleName;
        }
        public string Text { get; set; }
        public string RuleName { get; set; }
    }
}