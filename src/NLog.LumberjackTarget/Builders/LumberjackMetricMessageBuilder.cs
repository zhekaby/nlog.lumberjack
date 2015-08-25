using System;
using NLog.Targets.Lumberjack.Loggers;


namespace NLog.Targets.Lumberjack.Builders
{
    public class LumberjackMetricMessageBuilder : LumberjackBaseMessageBuilder
    {
        private LumberjackMetricMessage measureMessage;

        public LumberjackMetricMessageBuilder(IMessageLogger logger, IComponentSettings settings) : base(logger, settings)
        {
            this.message = new LumberjackMetricMessage();
            this.measureMessage = (LumberjackMetricMessage)message;
        }

        public LumberjackMetricMessageBuilder Name(string name)
        {
            this.measureMessage.Name = name;
            return this;
        }

        public LumberjackMetricMessageBuilder Value(double value)
        {
            this.measureMessage.Value = value;
            return this;
        }

        public override void Commit()
        {
            base.Commit();
            this.measureMessage.UnixTimestamp = GetUnixTimeNow();
            this.logger.LogMeasure(this.measureMessage);
        }

        private long GetUnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}