namespace NLog.Targets.Lumberjack.Loggers
{
    public interface IMessageLogger
    {
        void LogMessage(LumberjackLogMessage logMessage);
        void LogAlert(LumberjackAlertMessage alertMessage);
        void LogMeasure(LumberjackMetricMessage measureMessage);
    }
}