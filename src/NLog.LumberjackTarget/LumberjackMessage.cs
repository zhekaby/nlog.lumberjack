using System;
using System.Collections.Generic;

namespace NLog.Targets.Lumberjack
{
    public abstract class LumberjackMessageBase
    {
        public LumberjackMessageBase() { }

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

    public class LumberjackMessage : LumberjackMessageBase
    {
        public LumberjackMessage(string source, string applicationId, string component)
            : base(source, applicationId, component)
        {
            Level = LogLevel.Trace;
            Tags = new HashSet<string>();
        }

        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public HashSet<string> Tags { get; set; }
    }

    public class LumberjackMetricMessage : LumberjackMessageBase
    {
        public LumberjackMetricMessage(string source, string applicationId, string component, string name, double value, long timestamp)
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