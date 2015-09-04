using System;
using System.Collections.Generic;
using System.Linq;

namespace NLog.Logstash
{
    public class LumberjackMessageBuilder
    {
        public readonly NLog.Logger Logger;
        public LogstashMessage LogMessage;
        public LogstashAlertMessage AlertMessage;
        public LogstashMetricMessage MetricMessage;
        public readonly string Source, AppId, Component;
        public LumberjackMessageBuilder(NLog.Logger logger, string source, string appId, string component)
        {
            Logger = logger;
            Source = source;
            AppId = appId;
            Component = component;
        }
    }



    public static class LumberjackMessageBuilderExtensions
    {
        public static LumberjackMessageBuilder Log(this LumberjackMessageBuilder builder, LogLevel logLevel, string message)
        {
            builder.LogMessage = new LogstashMessage(builder.Source, builder.AppId, builder.Component, logLevel, message)
            { Tags = new HashSet<string>() };
            return builder;
        }

        public static LumberjackMessageBuilder Trace(this LumberjackMessageBuilder builder, string message)
        {
            return builder.Log(LogLevel.Trace, message);
        }
        public static LumberjackMessageBuilder Debug(this LumberjackMessageBuilder builder, string message)
        {
            return builder.Log(LogLevel.Debug, message);
        }
        public static LumberjackMessageBuilder Info(this LumberjackMessageBuilder builder, string message)
        {
            return builder.Log(LogLevel.Info, message);
        }
        public static LumberjackMessageBuilder Warn(this LumberjackMessageBuilder builder, string message)
        {
            return builder.Log(LogLevel.Warn, message);
        }
        public static LumberjackMessageBuilder Error(this LumberjackMessageBuilder builder, string message)
        {
            return builder.Log(LogLevel.Error, message);
        }
        public static LumberjackMessageBuilder Fatal(this LumberjackMessageBuilder builder, string message)
        {
            return builder.Log(LogLevel.Fatal, message);
        }

        public static LumberjackMessageBuilder WithTags(this LumberjackMessageBuilder builder, params string[] tags)
        {
            if (builder.LogMessage == null)
            {
                throw new ArgumentException("Message not set");
            }
            builder.LogMessage.Tags.UnionWith(tags);
            return builder;
        }
        public static LumberjackMessageBuilder WithTags(this LumberjackMessageBuilder builder, params Guid[] tags)
        {
            if (builder.LogMessage == null)
            {
                throw new ArgumentException("Message not set");
            }
            builder.LogMessage.Tags.UnionWith(tags.Select(i => i.ToString("N")));
            return builder;
        }
        public static LumberjackMessageBuilder WithField(this LumberjackMessageBuilder builder, string name, object value)
        {
            if (builder.LogMessage == null)
            {
                throw new ArgumentException("Message not set");
            }
            if (builder.LogMessage.Fields == null)
            {
                builder.LogMessage.Fields = new Dictionary<string, object> { { name, value } };
            }
            else
            {
                builder.LogMessage.Fields[name] = value;
            }
            return builder;
        }
        public static LumberjackMessageBuilder WithFields(this LumberjackMessageBuilder builder, IDictionary<string, object> fields)
        {
            if (builder.LogMessage == null)
            {
                throw new ArgumentException("Message not set");
            }
            if (builder.LogMessage.Fields == null)
            {
                builder.LogMessage.Fields = new Dictionary<string, object>(fields);
            }
            else
            {
                foreach (var kvp in fields)
                {
                    builder.LogMessage.Fields[kvp.Key] = kvp.Value;
                }
            }
            return builder;
        }
        public static LumberjackMessageBuilder Alert(this LumberjackMessageBuilder builder, string name, string message)
        {
            builder.AlertMessage = new LogstashAlertMessage(builder.Source, builder.AppId, builder.Component, name, message);
            return builder;
        }
        public static LumberjackMessageBuilder Measure(this LumberjackMessageBuilder builder, string name, double value)
        {
            var ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            builder.MetricMessage = new LogstashMetricMessage(builder.Source, builder.AppId, builder.Component, name, value, ts);
            return builder;
        }
        public static LumberjackMessageBuilder Measure(this LumberjackMessageBuilder builder, string name, double value, long timestamp)
        {
            builder.MetricMessage = new LogstashMetricMessage(builder.Source, builder.AppId, builder.Component, name, value, timestamp);
            return builder;
        }
        public static void Commit(this LumberjackMessageBuilder builder)
        {
            if (builder.LogMessage != null)
            {
                builder.Logger.Log(builder.LogMessage);
            }

            if(builder.AlertMessage != null)
            {
                builder.Logger.Alert(builder.AlertMessage);
            }

            if (builder.MetricMessage != null)
            {
                builder.Logger.Measure(builder.MetricMessage);
            }
        }
    }
}
