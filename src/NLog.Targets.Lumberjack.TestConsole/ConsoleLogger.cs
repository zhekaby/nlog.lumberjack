using NLog.Targets.Lumberjack.Builders;
using NLog.Targets.Lumberjack.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Targets.Lumberjack.TestConsole
{
    public class ConsoleLogger : IMessageLogger
    {
        private string GetLine(LumberjackMessageBase message)
        {
            return String.Format("source:{0}\tapplication_id:{1}\tcomponent:{2}\tmachine_name:{3}", message.Source, message.ApplicationId, message.Component, message.MachineName);
        }

        public void LogMessage(LumberjackLogMessage logMessage)
        {
            string bs = GetLine(logMessage);
            Console.WriteLine(String.Format("log_message:\t{0}\tlevel:{1}\ttags:{2}\tfields:{3}\tmessage:{4}",
                bs, logMessage.Level.Name, String.Join(",", logMessage.Tags), String.Join(",", logMessage.Fields.Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString())), logMessage.Message));
        }

        public void LogAlert(LumberjackAlertMessage alertMessage)
        {
            string bs = GetLine(alertMessage);
            Console.WriteLine(String.Format("alert_message:\t{0}\trule_name:{1}\ttext:{2}", bs, alertMessage.RuleName, alertMessage.Text));
        }

        public void LogMeasure(LumberjackMetricMessage measureMessage)
        {
            string bs = GetLine(measureMessage);
            Console.WriteLine(String.Format("log_message:\t{0}\tname:{1}\tvalue:{2}\ttimestamp:{3}", bs, measureMessage.Name, measureMessage.Value, measureMessage.UnixTimestamp));
        }
    }   
}
