using System.Collections.Generic;
using System.Linq;
using NLog.Targets.Lumberjack.Loggers;
using NLog.Targets.Lumberjack.Settings;

namespace NLog.Targets.Lumberjack.Builders
{
    public class LumberjackLogMessageBuilder : LumberjackBaseMessageBuilder
    {
        private LumberjackLogMessage logMessage;

        public LumberjackLogMessageBuilder(IMessageLogger logger, IComponentSettings settings) : base(logger, settings)
        {
            this.message = new LumberjackLogMessage();
            this.logMessage = (LumberjackLogMessage)message;
        }

        public LumberjackLogMessageBuilder Tags(params string[] tags)
        {
            if (this.logMessage.Tags == null && tags.Any())
            {
                this.logMessage.Tags = new HashSet<string>();
            }
            foreach (var tag in tags)
            {
                if (!this.logMessage.Tags.Contains(tag))
                {
                    this.logMessage.Tags.Add(tag);
                };
            }
            return this;
        }

        public LumberjackLogMessageBuilder Field(string key, object value)
        {
            if (this.logMessage.Fields == null)
            {
                this.logMessage.Fields = new Dictionary<string, object>();
            }
            if (!this.logMessage.Fields.ContainsKey(key))
            {
                this.logMessage.Fields.Add(key, value);
            }
            else
            {
                this.logMessage.Fields[key] = value;
            }
            return this;
        }

        public LumberjackLogMessageBuilder Message(string msg)
        {
            this.logMessage.Message = msg;
            return this;
        }

        public LumberjackLogMessageBuilder Level(LogLevel level)
        {
            this.logMessage.Level = level;
            return this;
        }

        public static implicit operator LumberjackLogMessage(LumberjackLogMessageBuilder builder)
        {
            return builder.logMessage;
        }

        public override void Commit()
        {
            base.Commit();
            this.logger.LogMessage(this.logMessage);
        }
    }
}
