using NLog.Targets.Lumberjack.Loggers;
using NLog.Targets.Lumberjack.Settings;

namespace NLog.Targets.Lumberjack.Builders
{
    public class LumberjackAlertMessageBuilder : LumberjackBaseMessageBuilder
    {
        private LumberjackAlertMessage alertMessage;

        public LumberjackAlertMessageBuilder(IMessageLogger logger, IComponentSettings settings) : base(logger, settings)
        {
            this.message = new LumberjackAlertMessage();
            this.alertMessage = (LumberjackAlertMessage)message;
        }

        public LumberjackAlertMessageBuilder Text(string text)
        {
            this.alertMessage.Text = text;
            return this;
        }

        public LumberjackAlertMessageBuilder Rule(string ruleName)
        {
            this.alertMessage.RuleName = ruleName;
            return this;
        }

        public override void Commit()
        {
            base.Commit();
            this.logger.LogAlert(this.alertMessage);
        }
    }
}