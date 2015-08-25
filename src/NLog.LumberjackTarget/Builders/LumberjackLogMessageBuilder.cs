using System.Collections.Generic;
using System.Linq;
using NLog.Targets.Lumberjack.Loggers;
using System;

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
            Array.ForEach(tags, t => this.logMessage.Tags.Add(t));
            return this;
        }

        public LumberjackLogMessageBuilder Field(string key, object value)
        {
            if (this.logMessage.Fields == null)
            {
                this.logMessage.Fields = new Dictionary<string, object>();
            }
            this.logMessage.Fields[key] = value;
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
