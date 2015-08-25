namespace NLog.Targets.Lumberjack.Builders
{
    public interface IComponentSettings
    {
        string ComponentName { get; }
        string ApplicationId { get; }
        string Source { get; }
        string MachineName { get; }
    }
}