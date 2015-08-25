using NLog.Targets.Lumberjack.Loggers;

namespace NLog.Targets.Lumberjack.Builders
{
    public abstract class LumberjackMessageBuilderBase
    {
        protected LumberjackMessageBase message;
        protected IMessageLogger logger;
        private IComponentSettings settings;

        public LumberjackMessageBuilderBase(IMessageLogger messageLogger, IComponentSettings settings)
        {
            this.logger = messageLogger;
            this.settings = settings;
        }

        public virtual void Commit()
        {
            this.message.ApplicationId = settings.ApplicationId;
            this.message.Component = settings.ComponentName;
            this.message.MachineName = settings.MachineName;
            this.message.Source = settings.Source;
        }
    }
}